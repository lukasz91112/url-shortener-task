using System.ComponentModel.DataAnnotations;

namespace UrlShortener.WebApplication.Models
{
    public class AddUrlViewModel
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; } = null!;
    }
}
