using System.Collections.Generic;
using System.Threading.Tasks;
using MergeMiner.Core.Network.Data;
using MergeMiner.Core.Network.Helpers;
using MergeMiner.Core.State.Data;

namespace _Proxy.Preloader
{
    public class RestAPI
    {
        private readonly RestClient _restClient;

        public RestAPI(
            RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<string> UserRegister()
        {
            return await _restClient.Get<string>("user/register");
        }
        
        public async Task<string> UserLogin(string deviceId)
        {
            return await _restClient.Get<string>($"user/login?deviceId={deviceId}");
        }
        
        public async Task<ConfigData> GetConfig(string token)
        {
            return await _restClient.Get<ConfigData>($"game/{token}/getConfig");
        }
        
        public async Task<GameState> GetState(string token)
        {
            return await _restClient.Get<GameState>($"game/{token}/getState");
        }
        
        public async Task<RestResponse> SpawnBox(string token)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/spawnBox");
        }

        public async Task<RestResponse> BuyMiner(string token, int level, Currency currency)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/buyMiner", new Dictionary<string, object>
            {
                { "level", level },
                { "currency", currency },
            });
        }
        
        public async Task<RestResponse> MergeMiners(string token, int slot1, int slot2)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/mergeMiners", new Dictionary<string, object>
            {
                { "slot1", slot1 },
                { "slot2", slot2 },
            });
        }
        
        public async Task<RestResponse> SwapMiners(string token, int slot1, int slot2)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/swapMiners", new Dictionary<string, object>
            {
                { "slot1", slot1 },
                { "slot2", slot2 },
            });
        }
        
        public async Task<RestResponse> RemoveMiner(string token, int slot)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/removeMiner", new Dictionary<string, object>
            {
                { "slot", slot },
            });
        }
        
        public async Task<RestResponse> GetFreeGem(string token)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/getFreeGem");
        }
        
        public async Task<RestResponse> Relocate(string token)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/relocate");
        }
        
        public async Task<RestResponse> SpeedUpBlueBox(string token)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/speedUpBlueBox");
        }
        
        public async Task<RestResponse> UseBonus(string token, string id)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/useBonus", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<SpinResponse> SpinWheel(string token, Currency currency)
        {
            return await _restClient.Post<SpinResponse>($"game/{token}/spinWheel", new Dictionary<string, object>
            {
                { "currency", currency }
            });
        }

        public async Task<RestResponse> MultiplyIncome(string token)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/multiplyIncome");
        }
        
        public async Task<RestResponse> BuyUpgrade(string token, string id)
        {
            return await _restClient.Post<RestResponse>($"game/{token}/buyUpgrade", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<RestResponse> PurchaseTest(string token, string id)
        {
            return await _restClient.Post<RestResponse>($"purchase/{token}/test", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<RestResponse> PurchaseTestSubscription(string token)
        {
            return await _restClient.Post<RestResponse>($"purchase/{token}/testSubscription");
        }
    }
}