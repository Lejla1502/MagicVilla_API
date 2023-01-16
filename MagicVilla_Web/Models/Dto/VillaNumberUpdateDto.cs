using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required(ErrorMessage ="Villa needs to be selected")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select villa")]
        public int VillaID { get; set; }
        public SelectList VillaList { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
