using BookingManagementRazorPages.Hubs;
using BookingManagerBlazorPages.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;
using Services.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookingManagementContext>(options => options.UseSqlServer("DefaultConnectionString"));

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddSignalR();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<SignalrSever>("/signalRServer");


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
