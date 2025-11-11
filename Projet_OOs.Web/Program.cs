using Projet_OOs.Web;
using Projet_OOs.Web.Components;
using Projet_OOS.Web.Core;      // Ajout pour un code plus clair
using Projet_OOS.Web.Services; // Ajout pour un code plus clair

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    client.BaseAddress = new("http://apiservice");
});

// ENREGISTREMENT UNIQUE ET CORRECT DES SERVICES
builder.Services.AddScoped<AlphaVantageService>();
builder.Services.AddScoped<BacktestingEngine>();
builder.Services.AddScoped<MetricsCalculator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();