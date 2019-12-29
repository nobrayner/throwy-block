using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    /// <summary>
    /// Fired when a player enters a trigger with a DeathZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredDeathZone"></typeparam>
    public class PlayerEnteredDeathZone : Simulation.Event<PlayerEnteredDeathZone> {
        public DeathZone DeathZone;

        public override void Execute() {
            Simulation.Schedule<PlayerDeath>(0);
        }
    }
}