namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Data.Models;

    public interface IMessageAttributePrototype
    {
        IMessageAttribute Materialize();
    }
}