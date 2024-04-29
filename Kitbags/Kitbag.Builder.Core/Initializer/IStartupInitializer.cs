namespace Kitbag.Builder.Core.Initializer;

public interface IStartupInitializer : IInitializer
{
    void AddInitializer(IInitializer initializer);
}