using System.Linq;
using ThrowyBlock.Mechanics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThrowyBlock.Model {
    [System.Serializable]
    public class MapModel {
        [Header("Debug")]
        public bool ShowRaycasts = true;

        [Header("Properties")]
        public CharacterActions[] Players;

        public Transform[] SpawnPoints;

        public Transform RespawnPoint;

        public Tilemap GroundMap;

        public LayerMask GroundLayer;
        public LayerMask PlayerLayer;
        public LayerMask DeathLayer;

        public CharacterActions GetPlayer(PlayerInfo playerInfo) {
            return Players.Where(e => e.GetComponent<PlayerInfo>() == playerInfo).FirstOrDefault();
        }
    }
}