using System.Net.Http.Json;

namespace Involved.HTF.Common;

public class HackTheFutureClient : HttpClient
{
    public HackTheFutureClient()
    {
        BaseAddress = new Uri("https://app-htf-2024.azurewebsites.net/");
    }

    public async Task Login(string teamname, string password)
    {
        var response = await GetAsync($"/api/team/token?teamname={teamname}&password={password}");
        if (!response.IsSuccessStatusCode)
            throw new Exception("You weren't able to log in, did you provide the correct credentials?");
        var token = await response.Content.ReadFromJsonAsync<AuthResponse>();
        DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjU1IiwibmJmIjoxNzMyMDA4NDI1LCJleHAiOjE3MzIwOTQ4MjUsImlhdCI6MTczMjAwODQyNX0.0OAWb6DuiGpUx4SwQpHFf-iubKMO251syh_xbPF_JnqBNpApY82CV2H5m7S1Mr_7VWGbZ0ixtXcfv5hgy0JCGQ");
    }
}

public class AuthResponse
{
    public string Token { get; set; }
}