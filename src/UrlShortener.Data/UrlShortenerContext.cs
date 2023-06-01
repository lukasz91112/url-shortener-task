using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<Url> Urls => Set<Url>();

        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options)
            :base(options)
        {

        }
    }
}
