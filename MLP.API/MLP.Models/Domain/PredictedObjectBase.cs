using System.ComponentModel.DataAnnotations;

namespace MLP.Models.Domain
{
    public abstract class PredictedObjectBase
    {
        [Required]
        public string ObjectName { get; set; }
    }
}
