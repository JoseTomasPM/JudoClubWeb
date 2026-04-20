using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using JudoClubWeb;
using JudoClubWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// URL base de la API — cámbiala cuando hagas deploy
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5187/")
});

// Registrar AuthService como Scoped (una instancia por sesión)
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();

await builder.Build().RunAsync();