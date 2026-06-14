namespace EmailSummarizerAPI.Models;

// ── REQUEST ──────────────────────────────────────────────
public class EmailSummaryRequest
{
    /// <summary>The raw email text to summarize.</summary>
    public string Email { get; set; } = string.Empty;
}

// ── RESPONSE ─────────────────────────────────────────────
public class EmailSummaryResponse
{
    public bool Success { get; set; }
    public string? Summary { get; set; }
    public string? Error { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}

// ── n8n payload (what we POST to the webhook) ────────────
public class N8nEmailPayload
{
    public string Email { get; set; } = string.Empty;
}
