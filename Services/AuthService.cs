using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace JudoClubWeb.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private string? _token;

    public bool IsLoggedIn => _token != null;
    public string? Role { get; private set; }

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    // Llamar esto al arrancar la app para restaurar la sesión
    public async Task InitAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "jwt");
        if (!string.IsNullOrEmpty(token))
            ApplyToken(token);
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result?.Token == null) return false;

        await _js.InvokeVoidAsync("localStorage.setItem", "jwt", result.Token);
        ApplyToken(result.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "jwt");
        _token = null;
        Role = null;
        _http.DefaultRequestHeaders.Authorization = null;
    }

    // Mantén el Logout síncrono como wrapper por compatibilidad con el navbar
    public void Logout() => _ = LogoutAsync();

    private void ApplyToken(string token)
    {
        _token = token;
        Role = ExtractRole(token);
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private static string? ExtractRole(string token)
    {
        try
        {
            var payload = token.Split('.')[1];
            var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(padded));
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var roleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            if (doc.RootElement.TryGetProperty(roleKey, out var role))
                return role.GetString();
            return null;
        }
        catch { return null; }
    }

    private record LoginResponse(string Token);
}