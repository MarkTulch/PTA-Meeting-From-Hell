using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWeapon : MeleeWeapon
{
    /// the offset position at which the projectile will spawn
    [Tooltip("the offset position at which the projectile will spawn")]
    public Vector3 ProjectileSpawnOffset = Vector3.zero;
}
