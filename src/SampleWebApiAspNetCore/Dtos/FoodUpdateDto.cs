using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiAspNetCore.Dtos
{
    public class FoodUpdateDto
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
    }
}
