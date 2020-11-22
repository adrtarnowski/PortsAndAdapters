namespace Kitbag.Builder.Core.Types
{
    public interface IStartupInitializer : IInitializer
    {
        void AddInitializer(IInitializer initializer);
    }
}