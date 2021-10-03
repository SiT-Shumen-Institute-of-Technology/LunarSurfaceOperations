namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes
{
    using System.Collections.Generic;

    public interface IMessagePrototype
    {
        string Text { get; }
        IReadOnlyCollection<IMessageAttributePrototype> Attributes { get; }
    }
}