using MergeMiner.Core.Network.Data;

namespace _Proxy.Preloader
{
    public class SessionData
    {
        public string Token { get; private set; }
        public GameState GameState { get; private set; }

        public void Setup(string token, GameState gameState)
        {
            Token = token;
            GameState = gameState;
        }
    }
}