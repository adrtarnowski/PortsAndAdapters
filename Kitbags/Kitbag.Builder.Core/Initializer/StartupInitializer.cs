using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kitbag.Builder.Core.Initializer
{
    public class StartupInitializer : IStartupInitializer
    {
        private readonly ISet<IInitializer> _initializers = new HashSet<IInitializer>();
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_initializers.Select(i => i.InitializeAsync()));
        }

        public void AddInitializer(IInitializer initializer)
        {
            _initializers.Add(initializer);
        }
    }
}