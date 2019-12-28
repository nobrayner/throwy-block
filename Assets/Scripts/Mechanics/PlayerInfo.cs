using UnityEngine;

namespace ThrowyBlock.Mechanics {
    public class PlayerInfo : MonoBehaviour {
        public int PlayerNumber = 1;
        public string Nickname = string.Empty;
        
        public string GetNickname() {
            if (Nickname != string.Empty) {
                return Nickname;
            }

            return "P" + PlayerNumber;
        }
    }
}
