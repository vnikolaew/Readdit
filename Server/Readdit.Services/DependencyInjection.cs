using System.Reflection;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Services.Data.Authentication;
using Readdit.Services.Data.Comments;
using Readdit.Services.Data.CommentVotes;
using Readdit.Services.Data.Common;
using Readdit.Services.Data.Communities;
using Readdit.Services.Data.Countries;
using Readdit.Services.Data.PostFeed;
using Readdit.Services.Data.Posts;
using Readdit.Services.Data.PostVotes;
using Readdit.Services.Data.Search;
using Readdit.Services.Data.Tags;
using Readdit.Services.Data.UserCommunities;
using Readdit.Services.Data.Users;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Messaging;
using Readdit.Services.Mapping;
using SendGrid.Extensions.DependencyInjection;

namespace Readdit.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddMappings()
            .AddImageStorage(configuration)
            .AddMailing(configuration)
            .AddJwtAuthentication(configuration)
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddTransient<IAuthenticationService, AuthenticationService>()
            .AddTransient<IUsersService, UsersService>()
            .AddTransient<ISearchService, SearchService>()
            .AddTransient<ICommunityService, CommunityService>()
            .AddTransient<IUserCommunityService, UserCommunityService>()
            .AddTransient<IPostsService, PostsService>()
            .AddTransient<IPostFeedService, PostFeedService>()
            .AddTransient<IPostVotesService, PostVotesService>()
            .AddTransient<ITagsService, TagsService>()
            .AddTransient<ICommentVotesService, CommentVotesService>()
            .AddTransient<ICommentsService, CommentsService>()
            .AddTransient<ICountryService, CountryService>()
            .AddTransient<IJwtService, JwtService>()
            .AddTransient<ICloudinaryService, CloudinaryService>();

    private static IServiceCollection AddImageStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = services.AddSettings<CloudinarySettings>(configuration);
        services.AddSingleton(new Cloudinary(new Account
        {
            Cloud = settings.Cloud,
            ApiKey = settings.ApiKey,
            ApiSecret = settings.ApiSecret
        }));
        
        return services;
    }

    private static IServiceCollection AddMailing(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = services.AddSettings<SendGridSettings>(configuration);
        services.AddSendGrid(options =>
        {
            options.ApiKey = settings.ApiKey;
        });
        
        return services.AddScoped<IEmailSender, SendGridEmailSender>();
    }

    private static TSettings AddSettings<TSettings>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TSettings : class, new()
    {
        var setting = new TSettings();
      
        configuration.Bind(typeof (TSettings).Name, setting);
        services.AddSingleton(setting);
      
        return setting;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = services.AddSettings<JwtSettings>(configuration);
        
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = settings.TokenValidationParameters;
            });
        
        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        MappingConfiguration.RegisterMappings(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}