namespace LunarSurfaceOperations.API.Converters
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using MongoDB.Bson;

    public class ObjectIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(ObjectId))
                return new BinderTypeModelBinder(typeof(ObjectIdModelBinder));

            return null;
        }
    }
}