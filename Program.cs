using FullStackApi.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("MongoDBConnectionString");
var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase("fullstackapi-web-database");



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Configure DbContext with MongoDB using options
builder.Services.AddDbContext<FullStackDbContext>();


//builder.Services.AddDbContext<FullStackDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("FullStackConnectionString")));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
