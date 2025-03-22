using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient with an increased timeout (60s)
builder.Services.AddHttpClient<StudentMarkSheetController>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(15);
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/StudentMarksheetUploadApi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
