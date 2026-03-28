using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Razor.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();

// register services from Razor.Services
builder.Services.AddSingleton<Razor.Services.Mapping.IMapper, Razor.Services.Mapping.SimpleMapper>();
builder.Services.AddTransient<Razor.Services.CondoService>();

// ensure Razor.Services project is available to the web app
// (Razor.Services is a project in the solution - if it's not already referenced, add a project reference in the .csproj)

// ADDED
builder.Services.AddDbContext<RazorAptDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorAptDbContext") ?? throw new InvalidOperationException("Connection string 'RazorAptDbContext' not found.")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
