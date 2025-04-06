using Application.Client.Pages;
using Application.Components;
using Application.Components.Account;
using Application.Shared.Models.User;
using Application.Shared.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
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
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Shared.Models.Data;
using Application.Shared.Models;
using Application.Helpers;
using Application.Shared.Models.Org;
using Application.Shared.Models.Sales;
using Application.Shared.Services.Sales;
using Application.Shared.Services.Org;
using Application.Shared.Services.Data;
using Application.Shared.Services.RealTime;
using Application.Shared.Models.Inventory;
using Application.Hubs;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddAuthenticationStateSerialization()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();




builder.Services.AddFluentUIComponents();

builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddAuthorizationCore();

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

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        options.Authority = builder.Configuration["AzureAd:Authority"];
        options.MetadataAddress = builder.Configuration["AzureAd:MetadataAddress"];
        options.CallbackPath = builder.Configuration["AzureAd:CallbackPath"];
        options.RequireHttpsMetadata = false;

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.SignedOutRedirectUri = builder.Configuration["AzureAd:SignedOutRedirectUri"];
        options.SignedOutCallbackPath = builder.Configuration["AzureAd:SignedOutCallbackPath"];
        options.ResponseType = OpenIdConnectResponseType.Code;


        // .NET 9 feature
        options.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;
        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
        options.TokenValidationParameters.RoleClaimType = "role";

    });



var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddRoleStore<RoleStore<IdentityRole, ApplicationDbContext>>()
    .AddUserStore<UserStore<ApplicationUser, IdentityRole, ApplicationDbContext>>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<StateContainer>();
builder.Services.AddScoped<ClientAuthenticationDetail>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();
builder.Services.AddScoped<IRealTimeDataService, RealTimeDataService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<ISalesChannelService, SalesChannelService>();
builder.Services.AddScoped<ISalesForecastService, SalesForecastService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IHostService, HostService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IDatabaseCredentialService, DatabaseCredentialService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<ITableStorageUsageService, TableStorageUsageService>();
builder.Services.AddScoped<ITableStorageUsageService, TableStorageUsageService>();
builder.Services.AddScoped<IQueryDetailService, QueryDetailService>();

builder.Services.AddScoped<QueryService<DataFile>>();
builder.Services.AddScoped<QueryService<SalesChannel>>();
builder.Services.AddScoped<QueryService<SalesForecastBySalesChannel>>();
builder.Services.AddScoped<QueryService<Item>>();
builder.Services.AddScoped<QueryService<Application.Shared.Models.Data.Host>>();
builder.Services.AddScoped<QueryService<Database>>();
builder.Services.AddScoped<QueryService<DatabaseCredential>>();
builder.Services.AddScoped<QueryService<Table>>();
builder.Services.AddScoped<QueryService<TableStorageUsage>>();
builder.Services.AddScoped<QueryService<QueryDetail>>();




// Add EmailSettings configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register EmailHelper as a singleton
builder.Services.AddSingleton<EmailHelper>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
    //options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
    //options.User.RequireUniqueEmail = true;

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

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<DataHub>("/datahub");
app.MapHub<NotificationHub<DataJob>>("/notification/datajob");
app.UseResponseCompression();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Application.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
