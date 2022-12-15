using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.Authentication;
using Readdit.Services.Data.Authentication.Models;
using Readdit.Services.Data.Countries;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Cloudinary.Models;
using Readdit.Services.External.Messaging;
using SignalRChat.Services.Security.Jwt;

namespace Readdit.Tests;

[TestFixture]
public class AuthenticationServiceTests
{
    private static AuthenticationService _authenticationService;
    
    private static Mock<IJwtService> _jwtServiceMock;
    private static Mock<UserManager<ApplicationUser>> _userManagerMock;
    private static Mock<ICloudinaryService> _cloudinaryServiceMock;
    private static Mock<IEmailSender> _emailSenderMock;
    private static Mock<ICountryService> _countryServiceMock;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _userManagerMock = TestUserManager<ApplicationUser>();
        _userManagerMock
            .Setup(um =>
                um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock
            .Setup(um =>
                um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock
            .Setup(um =>
                um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("token");

        _jwtServiceMock = new Mock<IJwtService>();
        _jwtServiceMock
            .Setup(js =>
                js.GenerateTokenForUser(
                    It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new JwtToken { Value = "Jwt Token", ExpiresAt = DateTime.Now.AddHours(1) });
        
        _cloudinaryServiceMock = new Mock<ICloudinaryService>();
        _cloudinaryServiceMock.Setup(cs =>
                cs.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ImageUploadResult(){ImagePublidId = "Id", AbsoluteImageUrl = "Url"});

        _emailSenderMock = new Mock<IEmailSender>();
        _emailSenderMock
            .Setup(es => es.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IEnumerable<EmailAttachment>>()
            ))
            .Returns(Task.CompletedTask);
        
        _countryServiceMock = new Mock<ICountryService>();
        _countryServiceMock.Setup(cs =>
            cs.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new Country()
        {
            Id = "Id",
            Name = "USA",
            Code = "US"
        });

        _authenticationService = new AuthenticationService(
            _userManagerMock.Object,
            _jwtServiceMock.Object,
            _cloudinaryServiceMock.Object,
            _emailSenderMock.Object,
            _countryServiceMock.Object
        );
    }
    
    public static Mock<UserManager<TUser>> TestUserManager<TUser>(
        IUserStore<TUser>? store = null) where TUser : class
    {
        store ??= new Mock<IUserStore<TUser>>().Object;
        var options = new Mock<IOptions<IdentityOptions>>();
        var idOptions = new IdentityOptions
        {
            Lockout =
            {
                AllowedForNewUsers = false
            }
        };

        options
            .Setup(o => o.Value)
            .Returns(idOptions);
        
        var userValidators = new List<IUserValidator<TUser>>();
        var validator = new Mock<IUserValidator<TUser>>();
        userValidators.Add(validator.Object);
        
        var pwdValidators = new List<PasswordValidator<TUser>> { new() };
        var passwordHasher = new Mock<IPasswordHasher<TUser>>();

        var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
            userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(), null,
            new Mock<ILogger<UserManager<TUser>>>().Object);
        
        validator
            .Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Verifiable();
        
        return new Mock<UserManager<TUser>>(
            store, options.Object,
            passwordHasher.Object,
            userValidators,
            pwdValidators, null, null, null, null);
    }

    [Test]
    public async Task Registering_NewUser_ShouldWorkAsExpected()
    {
      
        
        var registerInputModel = new RegisterInputModel
        {
            FirstName = "First name",
            LastName = "Last name",
            Country = "USA",
            EmailConfirmationUrl = "",
            Username = "Username",
            Email = "testuser@test.com",
            Gender = "Male",
            Password = "testpassword123"
        };

        var result = await _authenticationService.RegisterAsync(registerInputModel);
        
        Assert.That(result.Succeeded, Is.EqualTo(true));
        Assert.That(result.Token!.Value, Is.EqualTo("Jwt Token"));
        
        _userManagerMock.Verify(um => um.AddToRoleAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<string>()), Times.Once());
        _userManagerMock.Verify(um => um.GenerateEmailConfirmationTokenAsync(
            It.IsAny<ApplicationUser>()), Times.Once());
        _jwtServiceMock.Verify(um => um.GenerateTokenForUser(
            It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Once());
    }
    
    [Test]
    public async Task Registering_NewUser_WithUserManagerFailing_ShouldFail()
    {
        _userManagerMock.Setup(um =>
                um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = "Invalid credentials" }));
        
        _authenticationService = new AuthenticationService(
            _userManagerMock.Object,
            _jwtServiceMock.Object,
            _cloudinaryServiceMock.Object,
            _emailSenderMock.Object,
            _countryServiceMock.Object);
        
        var registerInputModel = new RegisterInputModel
        {
            FirstName = "First name",
            LastName = "Last name",
            Country = "USA",
            EmailConfirmationUrl = "",
            Username = "Username",
            Email = "testuser@test.com",
            Gender = "Male",
            Password = "testpassword123"
        };

        var result = await _authenticationService.RegisterAsync(registerInputModel);
        
        Assert.That(result.Succeeded, Is.EqualTo(false));
        Assert.That(result.Token?.Value, Is.EqualTo(null));
        Assert.That(result.Errors.First(), Is.EqualTo("Invalid credentials"));
        
        _userManagerMock.Verify(um => um.GenerateEmailConfirmationTokenAsync(
            It.IsAny<ApplicationUser>()), Times.Never());
        
        _jwtServiceMock.Verify(um => um.GenerateTokenForUser(
            It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never());
    }

    [Test]
    public async Task LoggingIn_User_ShouldWorkAsExpected()
    {
        _userManagerMock
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@test.com",
                Id = "TestId"
            });
        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock
            .Setup(um => um.GetRolesAsync(
                It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string>());
        _countryServiceMock
            .Setup(cs => cs.GetByUserAsync(It.IsAny<string>()))
            .ReturnsAsync(new Country { Name = "USA" });

        _authenticationService = new AuthenticationService(
            _userManagerMock.Object,
            _jwtServiceMock.Object,
            _cloudinaryServiceMock.Object,
            _emailSenderMock.Object,
            _countryServiceMock.Object);

        var result = await _authenticationService.PasswordLoginAsync(new LoginInputModel
        {
            Username = "",
            Password = ""
        });
        
        Assert.That(result.Succeeded, Is.EqualTo(true));
        Assert.That(result.Token!.Value, Is.EqualTo("Jwt Token"));
        
        _userManagerMock.Verify(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()), Times.Once());
        _countryServiceMock.Verify(cs => cs.GetByUserAsync(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public async Task LoggingIn_UserWithDuplicateName_ShouldFail()
    {
        _userManagerMock
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);
        
        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        
        _countryServiceMock
            .Setup(cs => cs.GetByUserAsync(It.IsAny<string>()))
            .ReturnsAsync(new Country { Name = "USA" });
        
        var result = await _authenticationService.PasswordLoginAsync(new LoginInputModel
        {
            Username = "",
            Password = ""
        });
                
        Assert.That(result.Succeeded, Is.EqualTo(false));
        Assert.That(result.Token, Is.EqualTo(null));
        Assert.That(result.Errors.First(), Is.EqualTo("User with the specified username was not found"));
                
        _userManagerMock.Verify(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _countryServiceMock.Verify(cs => cs.GetByUserAsync(It.IsAny<string>()), Times.Never());
    }
    
    [Test]
    public async Task LoggingIn_UserWithWrongPassword_ShouldFail()
    {
        _userManagerMock
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(() => new ApplicationUser
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@test.com",
                Id = "TestId"
            });
        
        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        
        _countryServiceMock
            .Setup(cs => cs.GetByUserAsync(It.IsAny<string>()))
            .ReturnsAsync(new Country { Name = "USA" });
        
        var result = await _authenticationService.PasswordLoginAsync(new LoginInputModel
        {
            Username = "",
            Password = ""
        });
                
        Assert.That(result.Succeeded, Is.EqualTo(false));
        Assert.That(result.Token, Is.EqualTo(null));
        Assert.That(result.Errors.First(), Is.EqualTo("Invalid credentials."));
                
        _userManagerMock.Verify(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _countryServiceMock.Verify(cs => cs.GetByUserAsync(It.IsAny<string>()), Times.Never());
        _jwtServiceMock.Verify(js => js.GenerateTokenForUser(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never());
    }
}