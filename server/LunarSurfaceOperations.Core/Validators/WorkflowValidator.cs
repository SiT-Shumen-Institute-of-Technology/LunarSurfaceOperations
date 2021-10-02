namespace LunarSurfaceOperations.Core.Validators
{
    using FluentValidation;
    using LunarSurfaceOperations.Data.Models;

    public class WorkflowValidator : AbstractValidator<Workspace>
    {
        public WorkflowValidator()
        {
            this.RuleFor(x => x.Name).NotNull().WithMessage("Please name your workspace").Length(8, 32).WithMessage("Name must be between 8 and 32 symbols");
            this.RuleFor(x => x.Description).MaximumLength(1600).WithMessage("Description must be not more then 1600 symbols long");
        }
    }
}
