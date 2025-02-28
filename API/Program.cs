using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Application.Interfaces;
using Application.Services;
using API.Extensions;
using Application.Authorization;
using Application.Utils;
using DataAccess.Repositiories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CuratorsJournalDBContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    });

builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IJournalsRepository, JournalsRepository>();
builder.Services.AddScoped<ICuratorsRepository, CuratorsRepository>();
builder.Services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();
builder.Services.AddScoped<IContactPhonesRepository, ContactPhonesRepository>();
builder.Services.AddScoped<IHolidaysRepository, HolidaysRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICuratorsAppointmentHistoryRepository, CuratorsAppointmentHistoryRepository>();
builder.Services.AddScoped<IPagesRepository, PagesRepository>();

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IJournalsService, JournalsService>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
