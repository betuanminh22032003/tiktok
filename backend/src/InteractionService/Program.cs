using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Redis
var redisConnectionString = builder.Configuration["RedisConnectionString"];
if (string.IsNullOrEmpty(redisConnectionString))
{
    throw new InvalidOperationException("Redis connection string 'RedisConnectionString' not found.");
}
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint to like a video
app.MapPost("/videos/{id}/like", async (string id, IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    var likeCount = await db.StringIncrementAsync($"video:{id}:likes");
    return Results.Ok(new { videoId = id, likes = likeCount });
});

// Endpoint to view a video
app.MapPost("/videos/{id}/view", async (string id, IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    var viewCount = await db.StringIncrementAsync($"video:{id}:views");
    return Results.Ok(new { videoId = id, views = viewCount });
});

// Endpoint to get video interactions
app.MapGet("/videos/{id}/interactions", async (string id, IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    var likes = (long)await db.StringGetAsync($"video:{id}:likes");
    var views = (long)await db.StringGetAsync($"video:{id}:views");
    return Results.Ok(new { videoId = id, likes, views });
});


app.Run();
