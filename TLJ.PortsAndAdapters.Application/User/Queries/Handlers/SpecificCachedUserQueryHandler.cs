using System;
using System.Text;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Queries;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TLJ.PortsAndAdapters.Application.User.DTO;

namespace TLJ.PortsAndAdapters.Application.User.Queries.Handlers
{
    public class SpecificCachedUserQueryHandler : IQueryHandler<SpecificCachedUserQuery, UserDTO>
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = new DateTimeOffset(new DateTime(2089, 10, 17, 07, 00, 00), TimeSpan.Zero),
            SlidingExpiration = TimeSpan.FromSeconds(3600)
        };

        public SpecificCachedUserQueryHandler(
            IDistributedCache distributedCache,
            IQueryDispatcher queryDispatcher)
        {
            _distributedCache = distributedCache;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<UserDTO> HandleAsync(SpecificCachedUserQuery query)
        {
            var cacheItem = await _distributedCache.GetAsync(query.UserName);
            if (cacheItem != null)
            {
                var cacheItemAsString = Encoding.UTF8.GetString(cacheItem);
                var result = JsonConvert.DeserializeObject<UserDTO>(cacheItemAsString);
                return result!;
            }
            else
            {
                var dto = await _queryDispatcher.QueryAsync(new SpecificUserQuery{ UserName = query.UserName });
                if (dto != null && dto.FullDomainName != null)
                    await StoreInCache(dto);
                return dto!;
            }
        }
        
        public async Task StoreInCache(UserDTO entity)
        {
            string key = entity.FullDomainName ?? throw new ArgumentNullException(nameof(entity.FullDomainName));
            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity)),
                _cacheOptions);
        }
    }
}