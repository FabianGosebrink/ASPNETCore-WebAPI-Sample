using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class FoodCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Calories { get; set; }
        public DateTime Created { get; set; }
    }
}
