using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace addac03e2ac2f62d1a7e766874af16b3.Server.Models
{
    [Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required string Name { get; set; }

        public string? State { get; set; }
        public string? Country { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        public DateTime DateEstablished { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Population must be at least 1.")]
        public long? Population { get; set; }
    }

    public class UpdateCityDto
    {
        public int Rating { get; set; }
        public DateTime DateEstablished { get; set; }
        public long Population { get; set; }
    }
}
