﻿using JobHunt.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JobHunt.Infrastructure.Cloudinary;
using JobHunt.Infrastructure.Persistence;
using JobHunt.Infrastructure.Pagination;
using JobHunt.Infrastructure.DbUp;

namespace JobHunt.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDatabaseUpgrader, DatabaseUpgrader>();
        services.AddDbContext<IJobHuntDbContext, JobHuntDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        //configure cloudinary for storing the images in cloud
        services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));
        services.AddScoped<ICloudImageService, CloudinaryImageService>();

        //register the pagination service
        services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
        return services;
    }
}
