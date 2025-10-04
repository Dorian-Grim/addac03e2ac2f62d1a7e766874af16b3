using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace addac03e2ac2f62d1a7e766874af16b3.Server.Models
{
    public class CityBase
    {
        public string? Name { get; set; }

        public string? State { get; set; }
        public string? Country { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        public DateTime DateEstablished { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Population must be at least 1.")]
        public int? Population { get; set; }
    }


    [Table("City")]
    public class City : CityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
