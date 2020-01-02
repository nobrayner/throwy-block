using ThrowyBlock.Core;
using ThrowyBlock.Events;
using UnityEngine;

public class Turret : MonoBehaviour {
    public GameObject Projectile;
    public Sprite BlockSprite;
    [Tooltip("Time in Seconds")] public int ShootRate;

    float lastProjectileTime = 0;

    void Update() {
        if (Time.time - lastProjectileTime > ShootRate) {
            lastProjectileTime = Time.time;

            Simulation.Schedule<SpawnThrownBlock>(0f).SetSpawnDetails(Projectile, new Vector2(-4f, 8f), new Vector2(-1f, 1f), transform, BlockSprite);
        }
    }
}
