using MergeMiner.Core.Network.Data;
using MergeMiner.Core.State.Config;

namespace GameCore.Preloader
{
    public class SessionData
    {
        public string Token { get; private set; }
        public string Email { get; private set; }
        public GameConfig GameConfig { get; private set; }
        public GameState GameState { get; private set; }
        public bool Working { get; private set; }

        public void Setup(string token, string email, GameConfig gameConfig, GameState gameState)
        {
            Token = token;
            Email = email;
            GameConfig = gameConfig;
            GameState = gameState;
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetWorking(bool value)
        {
            Working = value;
        }
    }
}