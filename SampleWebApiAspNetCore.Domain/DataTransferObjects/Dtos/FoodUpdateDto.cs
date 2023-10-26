using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApiAspNetCore.Domain.DataTransferObjects.Dtos
{
    public class FoodUpdateDto
    {
        public string? Name { get; set; }
        public int Calories { get; set; }
        public string? Type { get; set; }
        public DateTime Created { get; set; }
    }
}
