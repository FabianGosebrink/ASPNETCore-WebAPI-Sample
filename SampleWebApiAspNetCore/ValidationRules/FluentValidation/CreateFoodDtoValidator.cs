using FluentValidation;
using SampleWebApiAspNetCore.Dtos;

namespace SampleWebApiAspNetCore.ValidationRules.FluentValidation
{
    public class CreateFoodDtoValidator:AbstractValidator<FoodCreateDto>
    {
        public CreateFoodDtoValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Calories).NotNull().NotEmpty();    
            
        }
    }
}
