namespace LunarSurfaceOperations.Core.Validators
{
    using System;
    using FluentValidation;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Validation.Contracts;

    public class MessageValidator : AbstractValidator<IMessagePrototype>
    {
        public MessageValidator(IExhaustiveValidator<IMessageAttributePrototype> attributePrototypeValidator)
        {
            this.RuleFor(x => x.Text).NotEmpty().WithMessage("Please enter some text.");

            if (attributePrototypeValidator is null)
                throw new ArgumentNullException(nameof(attributePrototypeValidator));

            var exhaustiveValidatorBridge = new ExhaustiveValidatorBridge<IMessagePrototype, IMessageAttributePrototype>(attributePrototypeValidator);
            this.RuleForEach(x => x.Attributes).SetAsyncValidator(exhaustiveValidatorBridge);
        }
    }
}