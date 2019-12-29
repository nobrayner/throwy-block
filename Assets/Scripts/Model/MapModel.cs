using System.Linq;
using ThrowyBlock.Mechanics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThrowyBlock.Model {
    [System.Serializable]
    public class MapModel {
        public Tilemap GroundMap;

        public CharacterMovement[] Players;

        public Transform[] SpawnPoints;

        public Transform RespawnPoint;

        public CharacterMovement GetPlayer(PlayerInfo playerInfo) {
            return Players.Where(e => e.GetComponent<PlayerInfo>() == playerInfo).FirstOrDefault();
        }
    }
}