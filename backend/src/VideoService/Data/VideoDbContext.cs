using Microsoft.EntityFrameworkCore;
using VideoService.Models;

namespace VideoService.Data
{
    public class VideoDbContext : DbContext
    {
        public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }
    }
}
