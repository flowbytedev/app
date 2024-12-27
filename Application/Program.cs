using Application.Client.Pages;
using Application.Components;
using Application.Components.Account;
using Application.Shared.Models.User;
using Application.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Application.Services;
using Application.Shared.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddAuthenticationStateSerialization()
    .AddInteractiveWebAssemblyComponents().AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddApiAuthorization();

const string MS_OIDC_SCHEME = "MicrosoftOidc";

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;


    }).AddCookie("Identity.Application")
    .AddCookie("Identity.External")
    //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)

    .AddOpenIdConnect(MS_OIDC_SCHEME, displayName: "Continue with Microsoft" , options =>
    {

        //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        options.Authority = builder.Configuration["AzureAd:Authority"];
        options.MetadataAddress = builder.Configuration["AzureAd:MetadataAddress"];
        //options.Instance = builder.Configuration["AzureAd:Instance"];
        //options.Domain = builder.Configuration["AzureAd:Domain"];
        //options.TenantId = builder.Configuration["AzureAd:TenantId"];
        options.CallbackPath = builder.Configuration["AzureAd:CallbackPath"];
        options.RequireHttpsMetadata = false;
        options.SaveTokens = false;
        options.SignedOutRedirectUri = builder.Configuration["AzureAd:SignedOutRedirectUri"];
        options.SignedOutCallbackPath = builder.Configuration["AzureAd:SignedOutCallbackPath"];
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Add(OpenIdConnectScope.OpenIdProfile);
        options.Scope.Add(OpenIdConnectScope.OfflineAccess);
        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "roles";
    });



var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<StateContainer>();
builder.Services.AddScoped<ClientAuthenticationDetail>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<IDataService, Application.Shared.Services.DataService>();
builder.Services.AddScoped<IRealTimeDataService, RealTimeDataService>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
});

// get the uri from the appsettings.json
var uri = builder.Configuration["BaseUri"];
//// Configure the HttpClient to include the user's access token when calling the API
builder.Services.AddHttpClient("Application.ServerAPI", client => client.BaseAddress = new Uri(uri));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Application.ServerAPI"));


builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<DataHub>("/datahub");
app.UseResponseCompression();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    //.AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Application.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
