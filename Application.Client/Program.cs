using Application.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Application.Shared.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

//builder.Services.AddScoped<GetClientAuthenticationDetail>();

//var uri = "https://localhost:7234";
//// Configure the HttpClient to include the user's access token when calling the API
//builder.Services.AddHttpClient("Application.ServerAPI", client => client.BaseAddress = new Uri(uri))
//    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Application.ServerAPI"));

// send the user clains with api call
//builder.Services.AddScoped(http => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddApiAuthorization();


builder.Services.AddScoped(http => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ClientAuthenticationDetail>();
builder.Services.AddScoped<StateContainer>();
builder.Services.AddScoped<IDataService, DataService>();
//builder.Services.AddScoped<IRealTimeDataService, RealTimeDataService>();


//builder.Services.AddSignalR();

//builder.Services.AddResponseCompression(opts =>
//{
//    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
//        ["application/octet-stream"]);

//});

await builder.Build().RunAsync();
