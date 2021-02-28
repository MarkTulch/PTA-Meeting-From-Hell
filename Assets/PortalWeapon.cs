using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This class handles the placing of portals by the Biology Teacher
    /// </summary>
    public class PortalWeapon : Weapon
    {

        [MMInspectorGroup("Portal Weapon", true, 23)]

        protected MMSimpleObjectPooler _objectPool;

        protected Vector3 _currentPosition;
        protected Teleporter _portal;
        [SerializeField] protected GameObject[] _portalContainer = new GameObject[2];
        protected int _portalCount = 0;


        /// <summary>
        /// On init we grab our pool and initialize our stuff
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
            _objectPool = this.gameObject.GetComponent<MMSimpleObjectPooler>();
        }

        /// <summary>
        /// When the weapon is used, we spawn a portal
        /// </summary>
        public override void ShootRequest()
        {
            // only two portals at a time may be placed 
            if (GetPortalCount() == 2)
            {
                return;
            }
            // we don't call base on purpose
            SpawnPortal();
        }

        /// <summary>
        /// Deactivates all portals
        /// </summary>
        protected virtual void DeactivatePortals()
        {
            for (int i = 0; i < GetPortalCount(); i++ )
            {
                _portalContainer[i] = null;
            }
        }

        /// <summary>
        /// Spawns a portal
        /// </summary>
        protected virtual void SpawnPortal()
        {
            _currentPosition = this.transform.position;

            // we pool a new portal
            GameObject nextGameObject = _objectPool.GetPooledGameObject();
            if (nextGameObject == null)
            {
                return;
            }

            // we setup our portal and activate it
            nextGameObject.transform.position = _currentPosition;
            _portal = nextGameObject.MMGetComponentNoAlloc<Teleporter>();
            nextGameObject.gameObject.SetActive(true);

            _portalContainer[GetPortalCount()-1] = nextGameObject;
            // If two portals are placed, link them together
            if (GetPortalCount() == 2)
            {
                Debug.Log("Portals connected!");
                _portalContainer[0].MMGetComponentNoAlloc<Teleporter>().Destination = _portalContainer[1].MMGetComponentNoAlloc<Teleporter>();
                _portalContainer[1].MMGetComponentNoAlloc<Teleporter>().Destination = _portalContainer[0].MMGetComponentNoAlloc<Teleporter>();
            }

            // we change our state
            WeaponState.ChangeState(WeaponStates.WeaponUse);
        }

        protected private int GetPortalCount()
        {
            return FindObjectsOfType<Teleporter>().Length;
        }
    }
}

