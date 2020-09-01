using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class ReferenceDataInputModel
    {
        public virtual int Id { get; set; }
        public string Comment { get; set; }
        public List<AttachmentInputModel> Attachments { get; set; }
    }
}