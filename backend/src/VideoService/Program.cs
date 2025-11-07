using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VideoService.Data;
using VideoService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VideoDbContext>(options =>
    options.UseInMemoryDatabase("VideoDb"));

var app = builder.Build();

// Create uploads directory if it doesn't exist
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint to upload a video
app.MapPost("/videos/upload", async (HttpRequest request, VideoDbContext db) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Expected a form content type.");

    var form = await request.ReadFormAsync();
    var file = form.Files.GetFile("file");
    var title = form["title"].ToString();

    if (file is null)
        return Results.BadRequest("File is required.");

    if (string.IsNullOrEmpty(title))
        return Results.BadRequest("Title is required.");

    var video = new Video
    {
        Id = Guid.NewGuid(),
        Title = title,
        FilePath = Path.Combine("uploads", $"{Guid.NewGuid()}_{file.FileName}"),
        CreatedAt = DateTime.UtcNow
    };

    using (var stream = new FileStream(video.FilePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    db.Videos.Add(video);
    await db.SaveChangesAsync();

    return Results.Created($"/videos/{video.Id}", video);
});

// Endpoint to get video feed with pagination
app.MapGet("/videos/feed", async (VideoDbContext db, int page = 1, int pageSize = 10) =>
{
    var videos = await db.Videos
        .OrderByDescending(v => v.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(videos);
});


app.Run();
