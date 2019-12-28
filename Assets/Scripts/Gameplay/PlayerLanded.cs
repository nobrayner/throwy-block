using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;

namespace ThrowyBlock.Gameplay {
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerLanded : Simulation.Event<PlayerLanded> {
        public PlayerMovement player;

        public override void Execute() {

        }
    }
}