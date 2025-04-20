using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationDI(builder.Configuration);

builder.Services.AddDbContext<MainDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure"));
});

builder.Services.AddDbContext<IdentityContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

});

builder.Services.AddScoped<ITokenService, GenerateToken>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddIdentity<Student, IdentityRole>(option =>
{
    option.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;

    option.Password.RequiredUniqueChars = 2;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 8;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();





//TokenConfiguration.ConfigurJWTToken(builder.Services, builder.Configuration);



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
