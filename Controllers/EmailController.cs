using EmailSummarizerAPI.Models;
using EmailSummarizerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailSummarizerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmailController : ControllerBase
{
    private readonly IEmailSummarizerService _summarizerService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(
        IEmailSummarizerService summarizerService,
        ILogger<EmailController> logger)
    {
        _summarizerService = summarizerService;
        _logger = logger;
    }

    /// <summary>
    /// POST api/email/summarize
    /// Sends an email to the n8n AI Agent and returns bullet-point summary.
    /// </summary>
    [HttpPost("summarize")]
    [ProducesResponseType(typeof(EmailSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmailSummaryResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EmailSummaryResponse), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Summarize([FromBody] EmailSummaryRequest request)
    {
        // ── Validation ────────────────────────────────────
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new EmailSummaryResponse
            {
                Success = false,
                Error = "Email text cannot be empty."
            });
        }

        if (request.Email.Length < 10)
        {
            return BadRequest(new EmailSummaryResponse
            {
                Success = false,
                Error = "Email text is too short to summarize."
            });
        }

        // ── Call Service (→ n8n → Groq AI) ───────────────
        try
        {
            _logger.LogInformation("Summarize request received ({Length} chars)", request.Email.Length);

            var summary = await _summarizerService.SummarizeAsync(request.Email);

            return Ok(new EmailSummaryResponse
            {
                Success = true,
                Summary = summary
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach n8n webhook");
            return StatusCode(StatusCodes.Status502BadGateway, new EmailSummaryResponse
            {
                Success = false,
                Error = "Could not reach the AI agent. Please try again later."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during summarization");
            return StatusCode(StatusCodes.Status500InternalServerError, new EmailSummaryResponse
            {
                Success = false,
                Error = "An unexpected error occurred."
            });
        }
    }

    /// <summary>
    /// GET api/email/health
    /// Quick health check.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health() =>
        Ok(new { status = "ok", service = "Email Summarizer API", timestamp = DateTime.UtcNow });
}
