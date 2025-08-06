using EvDb.Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JSON serialization options to serialize enums as strings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

EvDbStorageContext context = new EvDbStorageContext("workshop-2025-08", builder.Environment.EnvironmentName, "Users");
builder.Services.AddEvDb()
        .AddFundsFactory(c => c.UseMongoDBStoreForEvDbStream(), context);

builder.Services.AddEvDb()
        .AddFundsWithOutboxFactory(c => c.UseMongoDBStoreForEvDbStream(), context);

builder.Services.AddEvDb()
        .AddFundsWithBalanceFactory(c => c.UseMongoDBStoreForEvDbStream(), context)
        .DefaultSnapshotConfiguration(c => c.UseMongoDBForEvDbSnapshot(), context);

builder.Services.AddEvDb()
        .AddFundsWithOutboxWithViewsFactory(c => c.UseMongoDBStoreForEvDbStream(), context)
        .DefaultSnapshotConfiguration(c => c.UseMongoDBForEvDbSnapshot(), context);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

