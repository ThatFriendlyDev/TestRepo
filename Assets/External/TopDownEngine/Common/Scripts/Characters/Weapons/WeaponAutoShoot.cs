using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Adds this component on a weapon with a WeaponAutoAim (2D or 3D) and it will automatically shoot at targets after an optional delay
    /// To prevent/stop auto shoot, simply disable this component, and enable it again to resume auto shoot
    /// </summary>
    public class WeaponAutoShoot : MonoBehaviour
    {
        [Header("Auto Shoot")]
        /// the delay (in seconds) between acquiring a target and starting shooting at it
        [Tooltip("the delay (in seconds) between acquiring a target and starting shooting at it")]
        public float DelayBeforeShootAfterAcquiringTarget = 0.1f;
 
        protected Weapon _weapon;
        protected bool _hasWeaponAndAutoAim;
        protected float _targetAcquiredAt;
        protected Transform _lastTarget;
        
        /// <summary>
        /// On Awake we initialize our component
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }
        
        /// <summary>
        /// Grabs auto aim and weapon
        /// </summary>
        protected virtual void Initialization()
        { 
            _weapon = this.gameObject.GetComponent<Weapon>();
           
        }
        
        /// <summary>
        /// On Update we handle auto shoot
        /// </summary>
        protected virtual void Update()
        {
            HandleAutoShoot();
        }

        /// <summary>
        /// Checks if we have a target for enough time, and shoots if needed
        /// </summary>
        protected virtual void HandleAutoShoot()
        {
            if (!_hasWeaponAndAutoAim)
            {
                return;
            }
            if (Time.time - _targetAcquiredAt >= DelayBeforeShootAfterAcquiringTarget)
            {
                _weapon.WeaponInputStart();
            }
        }
    }    
}
