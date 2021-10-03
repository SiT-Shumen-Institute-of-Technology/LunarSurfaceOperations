namespace LunarSurfaceOperations.Core.Validators
{
    using FluentValidation;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public class UserValidator : AbstractValidator<IUserPrototype>
    {
        public UserValidator()
        {
            this.RuleFor(x => x.Email).NotNull().WithMessage("Please enter your email address").EmailAddress().WithMessage("Please enter a valid email adress");
            this.RuleFor(x => x.Username).NotEmpty().WithMessage("Please make sure you have written your username.").Length(8, 20).WithMessage("Username must be between 8 and 20 symbols long.");
            this.RuleFor(x => x.Password).NotEmpty().WithMessage("Ensure you have written your password.").Length(8,32).WithMessage("Password must be between 8 and 32 symbols long.").NotEqual(x => x.Username).WithMessage("Password must not match Username.").NotEqual(x=> x.Email).WithMessage("Password must not match email");
        }  
    }
}
