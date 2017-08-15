using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Models
{
    public class HouseDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
    }
}