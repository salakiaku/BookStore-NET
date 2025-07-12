using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models.Book
{
    public class BookCreateDTO
    {
        [Required(ErrorMessage ="Campo obrigatório")]
        public string Title { get; set; }
        public int Year { get; set; }

        public string? Isbn { get; set; }

        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        public int? AuthorId { get; set; }
    }
}
