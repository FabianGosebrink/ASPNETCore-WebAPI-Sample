using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Models
{
    public class HouseDto
    {
        public int Id { get; set; }

        [Required, MinLength(3)]
        public string Street { get; set; }

        [Required, MinLength(3)]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }
    }
}