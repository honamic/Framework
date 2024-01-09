using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Honamic.IdentityPlus.Razor.Extentions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddIdentityPlus(builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();
