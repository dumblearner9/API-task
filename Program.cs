using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/", HandlePostRequest);  // ← 関数名だけ指定してる

app.Run();

// ここから下に関数定義しておけばOK
IResult HandlePostRequest(RequestBody body, string? apiKey, ILogger<Program> logger)
{
    if (apiKey != "hogehoge") return Results.Unauthorized();
    logger.LogInformation("{question}", body.Question);
    return Results.Ok(new Response { Answer = "(´・ω・｀)" });
}

class RequestBody
{
    public string? Question { get; set; }
}

class Response
{
    public string? Answer { get; set; }
}