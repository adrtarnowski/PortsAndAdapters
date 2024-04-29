using System.Threading.Tasks;

namespace Kitbag.Builder.Core.Initializer;

public interface IInitializer
{
    Task InitializeAsync();
}