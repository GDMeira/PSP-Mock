using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapPost("/payments/pix", async (TransferStatus dto) => //PSP destino
{
  Console.WriteLine($"Processing payment from {dto.Origin.User.CPF} to {dto.Destiny.Key.Value}");
  var timeToWait = GenerateRandomTime();
  Console.WriteLine($"This operation will return in {timeToWait} ms");
  await Task.Delay(timeToWait);

  return GenerateRandomResponse(); // 10% de chance de erro
});

app.MapPatch("/payments/pix", (TransferStatusDTO dto) => //PSP origem
{
  Console.WriteLine($"Processing payment status id {dto.Id} to {dto.Status}");
  return Results.NoContent();
});

app.MapPost("/concilliation/pix", async (HttpContext context) => //PSP origem concilliation
{
    string requestBody;
    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
    {
        requestBody = await reader.ReadToEndAsync();
    }

    Console.WriteLine($"Request Body: {requestBody}");

    var dto = JsonSerializer.Deserialize<ConcilliationReqDTO>(requestBody);

    return Results.NoContent();
});



static int GenerateRandomTime()
{
  Random random = new();
  int lowPercentage = 5; // 5% das reqs são lentas
  int percentageChoice = random.Next(1, 101);
  if (percentageChoice <= lowPercentage) return random.Next(130000, 180000); // TODO: you can change
  else return random.Next(100, 500);
}

static IResult GenerateRandomResponse()
{
  Random random = new();
  int lowPercentage = 1; // 1% das reqs dão errado
  int percentageChoice = random.Next(1, 101);
  if (percentageChoice <= lowPercentage) return Results.UnprocessableEntity(); // TODO: you can change
  else return Results.Ok();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();