using Microsoft.AspNetCore.Http;
using Moq;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Users;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Cloudinary.Models;
using Readdit.Tests.Common;

namespace Readdit.Tests;

[TestFixture]
public class UsersServiceTests
{
    private static IRepository<ApplicationUser> _users;
    private static Mock<ICloudinaryService> _cloudinaryServiceMock;
    private static IRepository<Country> _countries;
    private static UsersService _usersService;

    [OneTimeSetUp]
    public void SetUp()
    {
        _users = DbRepositoryProvider.Get<ApplicationUser>();
        _countries = DbRepositoryProvider.Get<Country>();
        _cloudinaryServiceMock = new Mock<ICloudinaryService>();
        
        var context = InMemoryDbContextProvider.Instance;
        context.Database.EnsureDeleted();

        _usersService = new UsersService(_users, _cloudinaryServiceMock.Object, _countries);
    }
    
    [TearDown]
    public void TearDown()
    {
        var context = InMemoryDbContextProvider.Instance;
        context.Database.EnsureDeleted();
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldWork_WithInsertedUser()
    {
        const string userId = "TestId";
        
        _users.Add(new ApplicationUser { Id = userId, FirstName = "", LastName = ""});
        await _users.SaveChangesAsync();

        var user = await _usersService.GetUserByIdAsync(userId);
        
        Assert.That(userId, Is.Not.EqualTo(null));
        Assert.That(user!.Id, Is.EqualTo(userId));
    }
    
    [Test]
    public async Task GetUserByIdAsync_ShouldFail_WithNoSuchUser()
    {
        const string userId = "TestId2";
        
        _users.Add(new ApplicationUser { Id = userId, FirstName = "", LastName = ""});
        await _users.SaveChangesAsync();

        var user = await _usersService.GetUserByIdAsync("OtherId");
        Assert.That(user, Is.EqualTo(null));
    }
    [Test]
    public async Task UpdatingUserProfile_ShouldWork_AsExpected()
    {
        _cloudinaryServiceMock.Setup(cs => cs.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(new ImageUploadResult
            {
                AbsoluteImageUrl = "ImageUrl",ImagePublidId = "ImageId"
            });
        _cloudinaryServiceMock.Setup(cs => cs.DeleteFileAsync(
                It.IsAny<string>()))
            .ReturnsAsync(true);
        
        const string userId = "TestId3";
        const string newFirstName = "John";
        const string newLastName = "Doe";
        const string newGender = "Male";
        const string newAbout = "New About Content ...";
        const string newCountry = "Bulgaria";
        
        var context = InMemoryDbContextProvider.Instance;
        _users.Add(new ApplicationUser
        {
            Id = userId, FirstName = "", LastName = "",
            Profile = new UserProfile{CountryId = "", ProfilePictureUrl = "PictureUrl", ProfilePicturePublicId = ""}
        });
        _countries.Add(new Country { Name = "USA", Code = "US"});
        _countries.Add(new Country { Name = newCountry, Code = "BG"});
        
        await context.SaveChangesAsync();

        var user = await _usersService.UpdateUserProfileAsync(
            userId, newFirstName, newLastName, newGender, newCountry, newAbout,
            new FormFile(new MemoryStream(), 0, 1, "", "")
            {
                Headers = new HeaderDictionary(),
                ContentType = "",
            });
        
        Assert.That(user, Is.Not.EqualTo(null));
        Assert.That(user.Id, Is.EqualTo(userId));
        Assert.That(user.FirstName, Is.EqualTo(newFirstName));
        Assert.That(user.LastName, Is.EqualTo(newLastName));
        Assert.That(user.Profile.AboutContent, Is.EqualTo(newAbout));
        Assert.That(user.Profile.Country.Name, Is.EqualTo(newCountry));
        Assert.That(user.Profile.Gender, Is.EqualTo(Gender.Male));
        Assert.That(user.Profile.ProfilePictureUrl, Is.EqualTo("ImageUrl"));
        Assert.That(user.Profile.ProfilePicturePublicId, Is.EqualTo("ImageId"));
        
        _cloudinaryServiceMock
            .Verify(cs => cs.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once());
        _cloudinaryServiceMock
            .Verify(cs =>
                cs.DeleteFileAsync(It.IsAny<string>()), Times.Once());
    }
}