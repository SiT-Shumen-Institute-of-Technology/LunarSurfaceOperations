namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public interface IUserService : IBaseService<IUserPrototype, IUserLayout>
    {
    }
}