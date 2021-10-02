namespace LunarSurfaceOperations.Resources
{
    using System.Reflection;
    using System.Resources;
    using JetBrains.Annotations;

    public static class ResourceManagerBuilder
    {
        public static ResourceManager CreateResourceManager([NotNull] string category)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var shortAssemblyName = currentAssembly.GetName().Name;
            var baseName = $"{shortAssemblyName}.{category}";
            return new ResourceManager(baseName, Assembly.GetExecutingAssembly());
        }
    }
}