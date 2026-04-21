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

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<StudentDto>($"api/student/{id}");
        }
        catch
        {
            return null;
        }
    }


    public async Task<List<SessionDto>> GetStudentSessionsAsync(int studentId)
    {
        // Obtiene todas las sesiones y filtra las que tiene el alumno
        var all = await _http.GetFromJsonAsync<List<SessionDto>>("api/sessions")
                  ?? new List<SessionDto>();
        return all;
    }

}