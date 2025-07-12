using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models.Author
{
    public class AuthorUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(50)]
        public string LastName { get; set; }
        public string Bio { get; set; }
    }
}
