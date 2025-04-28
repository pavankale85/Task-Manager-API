using FluentValidation;
using TaskMgnrAPI.DTOs;

namespace TaskMgnrAPI.Validators
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            //RuleFor(x => x.Status).IsInEnumOrString("Pending", "InProgress", "Done");
            RuleFor(x => x.DueDate).GreaterThan(DateTime.Now);
        }
    }
}
