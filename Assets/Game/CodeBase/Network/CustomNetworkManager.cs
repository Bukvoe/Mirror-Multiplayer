using System;
using System.Collections.Generic;
using Game.CodeBase.Character;
using Mirror;

namespace Game.CodeBase.Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public event Action<Player> PlayerAdded;
        public event Action<Player> PlayerRemoved;

        private readonly List<Player> _players = new();

        public static CustomNetworkManager Instance => singleton as CustomNetworkManager;

        public string Nickname { get; set; } = string.Empty;

        public void RegisterPlayer(Player player)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);
                PlayerAdded?.Invoke(player);
            }
        }

        public void UnregisterPlayer(Player player)
        {
            if (_players.Remove(player))
            {
                PlayerRemoved?.Invoke(player);
            }
        }
    }
}
