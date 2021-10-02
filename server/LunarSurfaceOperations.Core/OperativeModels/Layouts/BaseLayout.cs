namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record BaseLayout : ILayout
    {
        public BaseLayout(ObjectId id)
        {
            this.Id = id;
        }

        public ObjectId Id { get; }
    }
}