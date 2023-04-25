﻿using MergeMiner.Core.Network.Data;
using MergeMiner.Core.State.Config;

namespace GameCore.Preloader
{
    public class SessionData
    {
        public string Token { get; private set; }
        public GameConfig GameConfig { get; private set; }
        public GameState GameState { get; private set; }
        public bool Started { get; private set; }

        public void Setup(string token, GameConfig gameConfig, GameState gameState)
        {
            Token = token;
            GameConfig = gameConfig;
            GameState = gameState;
        }

        public void Start()
        {
            Started = true;
        }
    }
}