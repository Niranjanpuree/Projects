using System.ComponentModel.DataAnnotations;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class YearInputModel : ReferenceDataInputModel
    {
        [Required]
        [Range(0001,9999,ErrorMessage ="year invalid")]
        public override int Id { get; set; }
    }
}