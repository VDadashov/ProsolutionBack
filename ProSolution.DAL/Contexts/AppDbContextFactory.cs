using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProSolution.DAL.Contexts;
using System;
using System.IO;

namespace App.DAL.Presistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions => mySqlOptions.EnableStringComparisonTranslations());

            //optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options, null);
        }
    }
}
