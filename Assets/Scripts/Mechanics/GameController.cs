using ThrowyBlock.Core;
using ThrowyBlock.Model;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    public class GameController : MonoBehaviour {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public MapModel model = Simulation.GetModel<MapModel>();

        void OnEnable() {
            Instance = this;
        }

        void OnDisable() {
            if(Instance == this)
                Instance = null;
        }

        void Update() {
            if(Instance == this)
                Simulation.Tick();
        }
    }
}