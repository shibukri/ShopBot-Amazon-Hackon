using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.Interfaces;
using Employee_Management_System.ServiceFilters;
using Employee_Management_System.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employee_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IEmployeeBasicService, EmployeeBasicService>();
            builder.Services.AddScoped<IEmployeeAdditionalService , EmployeeAdditionalService>();
            builder.Services.AddScoped<ICosmosDBService, CosmosDBService>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddScoped<BuildEmployeeFilter>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
