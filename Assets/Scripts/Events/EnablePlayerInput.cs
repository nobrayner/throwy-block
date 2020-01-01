﻿using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;

namespace ThrowyBlock.Events {
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput> {
        CharacterActions Player;

        public override void Execute() {
            Player.ControlEnabled = true;
        }

        public void SetPlayer(CharacterActions player) {
            Player = player;
        }
    }
}
