using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;
using Services.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BookingManagementContext>(options => options.UseSqlServer("DefaultConnectionString"));
        builder.Services.AddRazorPages();
        builder.Services.AddSession();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ISlotService, SlotService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<ICampusService, CampusService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IApprovalHistoryService, ApprovalHistoryService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        builder.Services.AddAuthentication("Cookies")
            .AddCookie(options =>
            {
                options.LoginPath = "/Index";  
                options.LogoutPath = "/Index";
                options.AccessDeniedPath = "/Error";
            });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Manager", policy => policy.RequireRole("1"));  
            options.AddPolicy("Role2", policy => policy.RequireRole("2"));
            options.AddPolicy("Teacher", policy => policy.RequireRole("3"));
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseSession();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); 
        app.UseAuthorization(); 
        app.MapRazorPages();

 

        app.Run();
    }
}
