using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using JudoClubWeb;
using JudoClubWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://judoclubapi.onrender.com","http://localhost:5187")
});

await builder.Build().RunAsync();