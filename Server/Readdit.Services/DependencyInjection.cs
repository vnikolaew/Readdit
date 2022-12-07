using System.Reflection;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Services.Data.Authentication;
using Readdit.Services.Data.Comments;
using Readdit.Services.Data.CommentVotes;
using Readdit.Services.Data.Communities;
using Readdit.Services.Data.Posts;
using Readdit.Services.Data.PostVotes;
using Readdit.Services.Data.Tags;
using Readdit.Services.Data.UserCommunities;
using Readdit.Services.Data.Users;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.Mapping;

namespace Readdit.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddImageStorage(configuration)
            .AddJwtAuthentication(configuration)
            .AddTransient<IAuthenticationService, AuthenticationService>()
            .AddTransient<IUsersService, UsersService>()
            .AddTransient<ICommunityService, CommunityService>()
            .AddTransient<IUserCommunityService, UserCommunityService>()
            .AddTransient<IPostsService, PostsService>()
            .AddTransient<IPostVotesService, PostVotesService>()
            .AddTransient<ITagsService, TagsService>()
            .AddTransient<ICommentVotesService, CommentVotesService>()
            .AddTransient<ICommentsService, CommentsService>()
            .AddTransient<IJwtService, JwtService>()
            .AddTransient<ICloudinaryService, CloudinaryService>()
            .AddMappings();

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