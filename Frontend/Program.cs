using Frontend.Components;
using Frontend.Security;
using Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7059/") });
builder.Services.AddScoped<JournalState>();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<CookiesService>();
builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<APIService>();
builder.Services.AddScoped<ResourceService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddScheme<CustomOptions, JwtAuthenticationHandler>(
        "JWTAuth", option => { }
    );

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
