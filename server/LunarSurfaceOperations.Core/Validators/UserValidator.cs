using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentValidation;
using FluentValidation.Validators;
using LunarSurfaceOperations.Data.Models;
using Xunit;

namespace LunarSurfaceOperations.Core.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
            this.RuleFor(x => x.Email).NotNull().WithMessage("Please enter your email adress").EmailAddress().WithMessage("Please enter a valid email adress");
            this.RuleFor(x => x.Username).NotEmpty().WithMessage("Please make sure you have written your username.").Length(8, 20).WithMessage("Username must be between 8 and 20 symbols long.");
            this.RuleFor(x => x.Password).NotEmpty().WithMessage("Ensure you have written your password.").Length(8,32).WithMessage("Password must be between 8 and 32 symbols long.").NotEqual(x => x.Username).WithMessage("Password must not match Username.").NotEqual(x=> x.Email).WithMessage("Password must not match email");
        }
    
    }




}
