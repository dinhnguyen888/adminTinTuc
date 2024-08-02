using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class AccountService
{
    private readonly HttpClient _httpClient;

    public AccountService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7161/api/");
    }

    public async Task<string> RegisterAccountAsync(AccountRegistrationDto accountDto)
    {
        var json = JsonConvert.SerializeObject(accountDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Register/register", content);

        if (response.IsSuccessStatusCode)
        {
            return "Account registered successfully.";
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return $"Error registering account: {response.StatusCode} - {errorContent}";
        }
    }
}

public class AccountRegistrationDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Roles { get; set; }
}
