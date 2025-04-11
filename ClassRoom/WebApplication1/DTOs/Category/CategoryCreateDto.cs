using FluentValidation;

namespace WebApplication1.DTOs.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }

    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

        }
    }

}



