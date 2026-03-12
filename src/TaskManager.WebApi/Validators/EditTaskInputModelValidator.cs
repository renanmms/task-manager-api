using FluentValidation;
using TaskManager.WebApi.DTOs;

namespace TaskManager.WebApi.Validators
{
    public class EditTaskInputModelValidator : AbstractValidator<EditTaskInputModel>
    {
        public EditTaskInputModelValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .MinimumLength(15)
                .MaximumLength(60); 
        }
    }
}