using System.Collections.Generic;
using Game.CodeBase.Character;
using Mirror;

namespace Game.CodeBase.Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public readonly List<Player> Players = new();

        public static CustomNetworkManager Singleton => singleton as CustomNetworkManager;

        public string Nickname { get; set; } = string.Empty;
    }
}
