using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;

        public ServiceAWSCache()
        {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>?> GetCochesFavoritosAsync()
        {
            string? jsonCoches = await this.cache.StringGetAsync("fav");
            if (jsonCoches == null)
            {
                return null;
            }
            else
            {
                List<Coche> coches = new List<Coche>();
                coches = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches)!;
                return coches;
            }
        }

        public async Task AddFav(Coche coche)
        {
            List<Coche>? coches = await GetCochesFavoritosAsync();
            if (coches == null)
            {
                coches = new List<Coche>();
            }
            coches.Add(coche);
            string jsonCoches = JsonConvert.SerializeObject(coches);
            await this.cache.StringSetAsync("fav", jsonCoches, TimeSpan.FromMinutes(15));
        }

        public async Task RemoveFav(int id)
        {
            List<Coche>? coches = await GetCochesFavoritosAsync();
            if (coches == null)
            {
                return;
            }
            Coche? cocheRemove = coches.FirstOrDefault(coche => coche.IdCoche.Equals(id));
            if (cocheRemove == null)
            {
                return;
            }
            if (coches.Count() <= 1)
            {
                await this.cache.KeyDeleteAsync("fav");
            }
            else
            {
                coches.Remove(cocheRemove);
                string jsonCoches = JsonConvert.SerializeObject(coches);
                await this.cache.StringSetAsync("fav", jsonCoches, TimeSpan.FromMinutes(15));
            }
        }
    }
}
