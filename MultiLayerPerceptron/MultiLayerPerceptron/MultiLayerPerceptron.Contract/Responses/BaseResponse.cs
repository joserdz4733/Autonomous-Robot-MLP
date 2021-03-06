using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Responses
{
    public class BaseResponse<T>
    {
        [Required]
        public T Body { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}
