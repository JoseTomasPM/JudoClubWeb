using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace JudoClubWeb.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private string? _token;

    // Expone si hay sesión activa
    public bool IsLoggedIn => _token != null;

    // Expone el rol extraído del token (simple, sin librería)
    public string? Role { get; private set; }

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new
        {
            email,
            password
        });

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result?.Token == null)
            return false;

        // Guardar token en memoria
        _token = result.Token;

        // Extraer el rol del JWT manualmente (sin librerías extra)
        Role = ExtractRole(_token);

        // Añadir el token a todas las peticiones futuras del HttpClient
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        return true;
    }

    public void Logout()
    {
        _token = null;
        Role = null;
        _http.DefaultRequestHeaders.Authorization = null;
    }

    // Decodifica el payload del JWT para leer el rol
    // (sin instalar librerías — solo Base64)
    private static string? ExtractRole(string token)
    {
        try
        {
            var payload = token.Split('.')[1];
            // Añadir padding si es necesario
            var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(padded));
            var doc = System.Text.Json.JsonDocument.Parse(json);

            // La clave del rol en los tokens de Microsoft es esta URL larga
            var roleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            if (doc.RootElement.TryGetProperty(roleKey, out var role))
                return role.GetString();

            return null;
        }
        catch
        {
            return null;
        }
    }

    private record LoginResponse(string Token);
}