using System.Net.Http.Json;
using JudoClubWeb.Models;

namespace JudoClubWeb.Services;

public class StudentService
{
    private readonly HttpClient _http;

    public StudentService(HttpClient http) => _http = http;

    public async Task<List<StudentDto>> GetStudentsAsync(bool isAdmin)
    {
        var url = isAdmin ? "api/student" : "api/student/mine";
        var result = await _http.GetFromJsonAsync<List<StudentDto>>(url);
        return result ?? new List<StudentDto>();
    }
}