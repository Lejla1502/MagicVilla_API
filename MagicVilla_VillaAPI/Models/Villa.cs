using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   //to have Id automatically handled by DB
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public string Details { get; set; }
    }
}
