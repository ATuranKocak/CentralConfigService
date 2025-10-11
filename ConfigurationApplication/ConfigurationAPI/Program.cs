using ConfigurationLibrary.Interfaces;
using ConfigurationLibrary.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://mongo:27017"; 

// 2. Bu dinamik değeri MongoConfigurationRepository'ye gönder.
builder.Services.AddSingleton<IConfigurationRepository>(new MongoConfigurationRepository(connectionString));

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
