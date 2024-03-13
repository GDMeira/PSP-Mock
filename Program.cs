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

app.MapPost("/payments/pix", (TransferStatus dto) =>
{
  Console.WriteLine($"Processing payment from {dto.Origin.User.CPF} to {dto.Destiny.Key.Value}");
  var timeToWait = GenerateRandomTime();
  Console.WriteLine($"This operation will return in {timeToWait} ms");
  Thread.Sleep(timeToWait);

  return Results.Ok();
});

app.MapPatch("/payments/pix", (TransferStatusDTO dto) =>
{
  Console.WriteLine($"Processing payment status id {dto.Id} to {dto.Status}");
  return Results.NoContent();
});


static int GenerateRandomTime()
{
  Random random = new();
  int lowPercentage = 5; // 5% das reqs são lentas
  int percentageChoice = random.Next(1, 101);
  if (percentageChoice <= lowPercentage) return random.Next(60000, 90000); // TODO: you can change
  else return random.Next(100, 500);
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();