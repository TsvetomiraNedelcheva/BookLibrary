﻿using BookLibrary.Core.Services;
using BookLibrary.Infrastructure.Data;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Account account = new Account
        {
            ApiKey = builder.Configuration.GetValue<string>("ApiKey"),
            ApiSecret = builder.Configuration.GetValue<string>("ApiSecret"),
            Cloud = builder.Configuration.GetValue<string>("Cloud"),
        };

        var cloudinary = new Cloudinary(account);

        services.AddSingleton(cloudinary);

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IHomeService, HomeService>();
        return services;
    }

    public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        return services;
    }
}
