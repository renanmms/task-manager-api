using FluentValidation;
using TaskManager.WebApi.DTOs;

namespace TaskManager.WebApi.Validators
{
    public class NewTaskInputModelValidator : AbstractValidator<NewTaskInputModel>
    {
        public NewTaskInputModelValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .MinimumLength(15)
                .MaximumLength(60);
        }
    }
}