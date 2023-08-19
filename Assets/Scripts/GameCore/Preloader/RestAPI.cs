using System.Collections.Generic;
using System.Threading.Tasks;
using MergeMiner.Core.Network.Data;
using MergeMiner.Core.Network.Helpers;
using MergeMiner.Core.State.Data;

namespace GameCore.Preloader
{
    public class RestAPI
    {
        private readonly RestClient _restClient;

        public RestAPI(
            RestClient restClient)
        {
            _restClient = restClient;
        }
        
        public async Task<RestResponse<string>> Version()
        {
            return await _restClient.Get<string>("version");
        }
        
        public async Task<RestResponse<bool>> Status()
        {
            return await _restClient.Get<bool>("status");
        }
        
        public async Task<RestResponse<LoginData>> UserLogin(string deviceId)
        {
            return await _restClient.Get<LoginData>($"user/login?deviceId={deviceId}");
        }

        public async Task<RestResponse> Register(string token, string email, string password, string referralCode)
        {
            return await _restClient.Post("user/registerEmail", new Dictionary<string, object>
            {
                { "token", token },
                { "email", email },
                { "password", password },
                { "referralCode", referralCode }
            });
        }
        
        public async Task<RestResponse> Recover(string email)
        {
            return await _restClient.Post("user/recoverEmail", new Dictionary<string, object>
            {
                { "email", email }
            });
        }
        
        public async Task<RestResponse> Delete(string deviceId)
        {
            return await _restClient.Post("user/delete", new Dictionary<string, object>
            {
                { "deviceId", deviceId }
            });
        }
        
        public async Task<RestResponse<string>> RestoreByEmail(string deviceId, string email, string password)
        {
            return await _restClient.Post<string>("user/restoreByEmail", new Dictionary<string, object>
            {
                { "deviceId", deviceId },
                { "email", email },
                { "password", password }
            });
        }
        
        public async Task<RestResponse<ConfigSummary>> GetConfig(string token)
        {
            return await _restClient.Get<ConfigSummary>($"game/{token}/getConfig");
        }
        
        public async Task<RestResponse<GameState>> GetState(string token)
        {
            return await _restClient.Get<GameState>($"game/{token}/getState");
        }
        
        public async Task<RestResponse<GameCoreResponse>> SpawnBox(string token)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/spawnBox");
        }

        public async Task<RestResponse<GameCoreResponse>> BuyMiner(string token, int level, Currency currency)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/buyMiner", new Dictionary<string, object>
            {
                { "level", level },
                { "currency", currency },
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> MergeMiners(string token, int slot1, int slot2)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/mergeMiners", new Dictionary<string, object>
            {
                { "slot1", slot1 },
                { "slot2", slot2 },
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> SwapMiners(string token, int slot1, int slot2)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/swapMiners", new Dictionary<string, object>
            {
                { "slot1", slot1 },
                { "slot2", slot2 },
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> RemoveMiner(string token, int slot)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/removeMiner", new Dictionary<string, object>
            {
                { "slot", slot },
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> GetFreeGem(string token)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/getFreeGem");
        }
        
        public async Task<RestResponse<GameCoreResponse>> Relocate(string token)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/relocate");
        }
        
        public async Task<RestResponse<GameCoreResponse>> SpeedUpBlueBox(string token)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/speedUpBlueBox");
        }
        
        public async Task<RestResponse<GameCoreResponse>> UseBonus(string token, string id)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/useBonus", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<RestResponse<SpinResponse>> SpinWheel(string token, Currency currency)
        {
            return await _restClient.Post<SpinResponse>($"game/{token}/spinWheel", new Dictionary<string, object>
            {
                { "currency", currency }
            });
        }

        public async Task<RestResponse<GameCoreResponse>> GetOfflineIncome(string token, bool multiply)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/getOfflineIncome", new Dictionary<string, object>
            {
                { "multiply", multiply }
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> BuyUpgrade(string token, string id)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/buyUpgrade", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> CollectMission(string token, string id)
        {
            return await _restClient.Post<GameCoreResponse>($"game/{token}/collectMission", new Dictionary<string, object>
            {
                { "id", id }
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> Purchase(string token, string id, string purchaseToken)
        {
            return await _restClient.Post<GameCoreResponse>($"purchase/{token}/purchase", new Dictionary<string, object>
            {
                { "id", id },
                { "purchaseToken", purchaseToken }
            });
        }
        
        public async Task<RestResponse<GameCoreResponse>> Subscription(string token, string subscriptionId, string purchaseToken)
        {
            return await _restClient.Post<GameCoreResponse>($"purchase/{token}/subscription", new Dictionary<string, object>
            {
                { "subscriptionId", subscriptionId },
                { "purchaseToken", purchaseToken }
            });
        }
    }
}