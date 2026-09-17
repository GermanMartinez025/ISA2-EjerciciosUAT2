using Controllers.Companies;
using Controllers.Devices;
using Controllers.Homes;
using Controllers.Importers;
using Controllers.Sessions;
using Controllers.Users;
using Controllers.Validations;
using ControllersInterfaces.Companies;
using ControllersInterfaces.Devices;
using ControllersInterfaces.Homes;
using ControllersInterfaces.Importers;
using ControllersInterfaces.Sessions;
using ControllersInterfaces.Users;
using ControllersInterfaces.Validations;
using DataAccess;
using IRepositories.Repositories.CompanyRepositories;
using IRepositories.Repositories.DeviceRepositories;
using IRepositories.Repositories.HomesRepositories;
using IRepositories.Repositories.SessionRepositories;
using IRepositories.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelInterface.Devices;
using Models.Company;
using ModelValidation;
using Repositories.CompanyRepositories;
using Repositories.DeviceRepositories;
using Repositories.Homes;
using Repositories.SessionRepositories;
using Repositories.UserRepositories;
using Services.Companys;
using Services.Devices;
using Services.Homes;
using Services.Importers;
using Services.Sessions;
using Services.Users;
using Services.Validations;
using ServicesInterfaces.Company;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Importers;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;
using ServicesInterfaces.Validations;


namespace InjectionService;

public static class DependencyInjector
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configurations)
    {
        var connectionString = configurations.GetConnectionString("AppDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Missing connection string");
        }
        services.AddDbContext<DbContext, AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
    
    public static void AddUserDependency(this IServiceCollection services)
    {
        services.AddScoped<IHomeUserRepository, HomeUserRepository>();
        services.AddScoped<IHomeUserService, HomeUserService>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IUserController, UserController>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyOwnerRepository, CompanyOwnerRepository>();
        services.AddScoped<ICompanyOwnerService, CompanyOwnerService>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
    public static void AddSessionDependency(this IServiceCollection services)
    {
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ISessionController, SessionController>();
    }
    
    public static void AddHomeDependency(this IServiceCollection services)
    {
        services.AddScoped<IHomeRepository, HomeRepository>();
        services.AddScoped<IHomeController, HomeController>();
        services.AddScoped<IHomeServices, HomeService>();
        services.AddScoped<IHomeMemberService, HomeMemberService>();
        services.AddScoped<IHomeDeviceRepository, HomeDeviceRepository>();
        services.AddScoped<IHomeDeviceServices, HomeDeviceServices>();
        services.AddScoped<IHomeDeviceController, HomeDeviceController>();
        services.AddScoped<IDeviceController, DeviceController>();
        services.AddScoped<IRoomServices, RoomServices>();
        services.AddScoped<IRoomRepository, RoomRepository>();
    }
    
    public static void AddCompanyDependency(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICompanyController, CompanyController>();
        services.AddScoped<ICompanyOwnerService, CompanyOwnerService>();
        services.AddScoped<ICompanyOwnerRepository, CompanyOwnerRepository>();
        services.AddScoped<IDeviceServices, DeviceServices>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<ICameraRepository, CameraRepository>();
        services.AddScoped<IValidationProvider, ValidationProvider>();
        services.AddScoped<IValidationController, ValidationController>();
        services.AddScoped<IImporterController, ImporterController>();
        services.AddScoped<IImporterService>(provider =>
        {
            var path = "../Importers";
            return new ImporterService(path);
        });
    }
    
}