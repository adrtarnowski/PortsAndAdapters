using System.Threading.Tasks;

namespace Kitbag.Builder.Core.Types
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}