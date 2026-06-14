using EmailSummarizerAPI.Models;
using System.Text;
using System.Text.Json;

namespace EmailSummarizerAPI.Services;

public interface IEmailSummarizerService
{
    Task<string> SummarizeAsync(string emailText);
}

public class EmailSummarizerService : IEmailSummarizerService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<EmailSummarizerService> _logger;

    public EmailSummarizerService(
        HttpClient httpClient,
        IConfiguration config,
        ILogger<EmailSummarizerService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<string> SummarizeAsync(string emailText)
    {
        var webhookUrl = _config["N8n:WebhookUrl"]
            ?? throw new InvalidOperationException("N8n:WebhookUrl is not configured.");

        var payload = new { email = emailText };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(webhookUrl, content);

        var raw = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("Status: {Status}", response.StatusCode);
        _logger.LogInformation("Raw response: '{Raw}'", raw);
        _logger.LogInformation("Content length: {Len}", raw.Length);

        return raw.Trim();
    }
}
