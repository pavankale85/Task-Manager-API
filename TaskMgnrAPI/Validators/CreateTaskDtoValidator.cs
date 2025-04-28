using FluentValidation;
using TaskMgnrAPI.DTOs;

namespace TaskMgnrAPI.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            //object value = RuleFor(x => x.Status).IsInEnumOrString("Pending", "InProgress", "Done");
            RuleFor(x => x.DueDate).GreaterThan(DateTime.Now).WithMessage("Due date must be in the future.");
        }
    }
}
