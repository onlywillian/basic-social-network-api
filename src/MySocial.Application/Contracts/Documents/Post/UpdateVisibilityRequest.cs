using MySocial.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace MySocial.Application.Contracts.Documents.Post
{
    public class UpdateVisibilityRequest
    {
        [Required]
        [EnumDataType(typeof(Visibilitys))]
        public Visibilitys visibility { get; set; }
    }
}
