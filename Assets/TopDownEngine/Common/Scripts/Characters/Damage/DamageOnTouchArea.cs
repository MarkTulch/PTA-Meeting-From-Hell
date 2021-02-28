using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using System;
using MoreMountains.Feedbacks;

namespace MoreMountains.TopDownEngine
{
    public class DamageOnTouchArea : DamageOnTouch
    {
        [Header("Explosion size")]
        // size of the explosion
        [Tooltip("the radius of the explosion")]
        public float _radius = 1.0f;

        /// <summary>
        /// Describes what happens when colliding with a damageable object
        /// </summary>
        /// <param name="health">Health.</param>
        override
        protected void OnCollideWithDamageable(Health health)
        {
            _colliderTopDownController = health.gameObject.MMGetComponentNoAlloc<TopDownController>();
            _colliderCharacterHandleWeapon = health.gameObject.MMGetComponentNoAlloc<CharacterHandleWeapon>();
            _colliderRigidBody = health.gameObject.MMGetComponentNoAlloc<Rigidbody>();

            HitDamageableFeedback?.PlayFeedbacks(this.transform.position);

            _colliderHealth.Damage(DamageCaused, gameObject, InvincibilityDuration, InvincibilityDuration);

            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _radius, TargetLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.MMGetComponentNoAlloc<Health>()?.Damage(DamageCaused, gameObject, InvincibilityDuration, InvincibilityDuration);
            }


            if (DamageTakenEveryTime + DamageTakenDamageable > 0)
            {
                SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
            }
        }
    }
}
