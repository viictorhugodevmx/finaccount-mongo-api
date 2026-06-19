using FinAccountMongoApi.Repositories;
using FinAccountMongoApi.Services;
using FinAccountMongoApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<MovementRepository>();
builder.Services.AddScoped<MovementApplicationService>();
builder.Services.AddScoped<AccountSummaryService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
