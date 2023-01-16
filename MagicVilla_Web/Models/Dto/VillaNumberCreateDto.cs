using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberCreateDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select villa")]
        public int VillaID { get; set; }
        //public SelectList VillaList { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
