# 📧 Email Summarizer API — ASP.NET Core MVC

An ASP.NET Core Web API that connects to your **n8n AI Agent workflow** (powered by Groq) to summarize emails into bullet points.

## 🏗️ Architecture (MVC Pattern)

```
POST /api/email/summarize
         │
         ▼
  [EmailController]        ← Controller (C)
         │
         ▼
  [EmailSummarizerService] ← Service / Model (M)
         │
         ▼
  [n8n Webhook]            ← AI Agent (Groq)
         │
         ▼
  [JSON Response]          ← View/Output (V)
```

## 🚀 Setup & Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Your n8n workflow running (already done ✅)

### Steps

```bash
# 1. Navigate to project
cd EmailSummarizerAPI

# 2. Restore packages
dotnet restore

# 3. Run the API
dotnet run
```

Open your browser at: **http://localhost:5000** → Swagger UI loads automatically!

---

## 📡 API Endpoints

### POST `/api/email/summarize`
Summarizes an email using the n8n AI Agent.

**Request Body:**
```json
{
  "email": "Yesterday we completed phase 1 of the project. The client approved the UI design. Backend development starts next Monday."
}
```

**Success Response (200):**
```json
{
  "success": true,
  "summary": "- Completed phase 1 yesterday\n- Client approved the UI\n- Backend development starts Monday",
  "error": null,
  "processedAt": "2026-06-14T10:00:00Z"
}
```

**Error Response (400):**
```json
{
  "success": false,
  "summary": null,
  "error": "Email text cannot be empty.",
  "processedAt": "2026-06-14T10:00:00Z"
}
```

### GET `/api/email/health`
Health check endpoint.

```json
{
  "status": "ok",
  "service": "Email Summarizer API",
  "timestamp": "2026-06-14T10:00:00Z"
}
```

---

## 🧪 Test with Postman

```
POST http://localhost:5000/api/email/summarize
Content-Type: application/json

{
  "email": "Yesterday we completed phase 1. Client approved the UI. Backend starts Monday."
}
```

---

## ⚙️ Configuration

Edit `appsettings.json` to change the n8n webhook URL:

```json
{
  "N8n": {
    "WebhookUrl": "https://mennamahmoud0.app.n8n.cloud/webhook/email-summary"
  }
}
```

---

## 📁 Project Structure

```
EmailSummarizerAPI/
├── Controllers/
│   └── EmailController.cs      ← API endpoints (MVC Controller)
├── Models/
│   └── EmailModels.cs          ← Request/Response models (MVC Model)
├── Services/
│   └── EmailSummarizerService.cs ← Business logic, calls n8n
├── Properties/
│   └── launchSettings.json
├── Program.cs                  ← App entry point, DI setup
├── appsettings.json            ← Config (webhook URL)
└── EmailSummarizerAPI.csproj
```
