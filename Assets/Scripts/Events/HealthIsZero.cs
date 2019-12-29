using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using static ThrowyBlock.Core.Simulation;

namespace ThrowyBlock.Events {
    /// <summary>
    /// Fired when the player health reaches 0. This usually would result in a 
    /// PlayerDeath event.
    /// </summary>
    /// <typeparam name="HealthIsZero"></typeparam>
    public class HealthIsZero : Simulation.Event<HealthIsZero> {
        public Health health;

        public override void Execute() {
            Schedule<PlayerDeath>();
        }
    }
}