using BaseModule.Repository.User;
using DomainModule.BaseRepo;
using DomainModule.Repository;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.ActivityLog;
using DomainModule.RepositoryInterface.AuditLog;
using DomainModule.Service;
using DomainModule.ServiceInterface;
using DomainModule.ServiceInterface.Account;
using DomainModule.ServiceInterface.Email;
using InfrastructureModule.Repository;
using InfrastructureModule.Repository.ActivityLog;
using InfrastructureModule.Repository.AuditLog;
using Inventory.Repository.Implementation;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Services.Implementation;
using InventoryLibrary.Services.ServiceInterface;
using InventoryLibrary.Source.Repository.Interface;
using InventoryLibrary.Source.Repository;
using InventoryLibrary.Source.Services.Implementation;
using InventoryLibrary.Source.Services.ServiceInterface;
using InventoryLibrary.Source.Services;
using ServiceModule.Service;
using ServiceModule.Service.Email;
using System.Runtime.CompilerServices;
using NepaliDateConverter;


namespace WebApp.DiConfig
{
    public static class DiConfiguration
    {
        public static void  UseDIConfig(this IServiceCollection services)
        {
            UseRepository(services);
            UseService(services);
        }
        private static void UseRepository(IServiceCollection services)
        {
         services.AddScoped<UserRepositoryInterface,UserRepository>();
         services.AddScoped<RoleRepositoryInterface,RoleRepository>();
         services.AddScoped<IAuditLogRepository,AuditLogRepository>();
         services.AddScoped<IActivityLogRepository,ActivityLogRepository>();
         services.AddScoped<AppSettingsRepositoryInterface,AppSettingsRepository>();

            services.AddScoped<CustomerRepositoryInterface, CustomerRepository>();

            services.AddScoped<ItemRepositoryInterface, ItemRepository>();

            services.AddScoped<UnitRepositoryInterface, UnitRepository>();

            services.AddScoped<SaleRepositoryInterface, SaleRepository>();

            services.AddScoped<SaleDetailRepositoryInterface, SaleDetailRepository>();

            services.AddScoped<SupplierRepositoryInterface, SupplierRepository>();

            services.AddScoped<PurchaseRepositoryInterface, PurchaseRepository>();

            services.AddScoped<PurchaseDetailRepositoryInterface, PurchaseDetailRepository>();
            services.AddScoped<CustomerTransactionRepositoryInterface, CustomerTransactionRepository>();

        }
        private static void UseService(IServiceCollection services)
        {

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UserServiceInterface, UserService>();
            services.AddScoped<RoleServiceInterface, RoleService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
			services.AddTransient<IJWTTokenGenerator, JWTTokenGenerator>();
            services.AddScoped<AppSettingsServiceInterface, AppSettingsService>();
            services.AddScoped<CustomerTransactionServiceInterface, CustomerTransactionService>();
            services.AddScoped<PurchaseServiceInterface, PurchaseService>();
            services.AddScoped<SaleServiceInterface, SaleService>();
            services.AddScoped<UnitServiceInterface, UnitService>();
            services.AddScoped<SupplierServiceInterface, SupplierService>();
            services.AddScoped<ItemServiceInterface, ItemService>();
            services.AddScoped<CustomerServiceInterface, CustomerService>();

        }
    }
}
