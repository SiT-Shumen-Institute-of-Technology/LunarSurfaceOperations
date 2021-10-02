namespace LunarSurfaceOperations.API.Converters
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using MongoDB.Bson;

    public class ObjectIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);

            var value = result.FirstValue;
            if (string.IsNullOrWhiteSpace(value) || ObjectId.TryParse(value, out var id) == false)
                bindingContext.Result = ModelBindingResult.Failed();
            else
                bindingContext.Result = ModelBindingResult.Success(id);

            return Task.CompletedTask;
        }
    }
}