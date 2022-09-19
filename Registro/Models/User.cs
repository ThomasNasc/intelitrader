using System.ComponentModel.DataAnnotations;


namespace Registro.Models
{
    public class User
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string firstName { get; set; }
        public string? surName { get; set; }
        [Required]
        public int age { get; set; }
        public DateTime? dateOfCreation { get; set; }


    }
}
