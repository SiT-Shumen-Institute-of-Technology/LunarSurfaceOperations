namespace LunarSurfaceOperations.Core.Validators
{
    using FluentValidation;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;

    public class MessageStringAttributeValidator : AbstractValidator<MessageStringAttributePrototype>
    {
        public MessageStringAttributeValidator()
        {
            this.RuleFor(x => x.Value).NotEmpty();
            this.RuleFor(x => x.AttributeName).NotEmpty();
        }
    }
}