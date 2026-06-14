using EmailSummarizerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<IEmailSummarizerService, EmailSummarizerService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();