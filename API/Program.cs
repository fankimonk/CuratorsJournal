using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Application.Interfaces;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IJournalsService, JournalsService>();

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
