using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this class to a character so it can use weapons
    /// Note that this component will trigger animations (if their parameter is present in the Animator), based on 
    /// the current weapon's Animations
    /// Animator parameters : defined from the Weapon's inspector
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Handle Weapon")]
    public class CharacterHandleWeapon : CharacterAbility
    {
        [Header("Weapon")]
        [Tooltip("the initial weapon owned by the character")]
        public Weapon InitialWeapon;

        [Header("Feedbacks")]
        public MMFeedbacks WeaponUseFeedback;

        [Header("Binding")]
        public Transform WeaponAttachment;
        public Transform ProjectileSpawn;

        /// the weapon currently equipped by the Character
        [MMReadOnly]
        [Tooltip("the weapon currently equipped by the Character")]
        public Weapon CurrentWeapon;
 
        /// an animator to update when the weapon is used
        public Animator CharacterAnimator { get; set; }
        /// the weapon's weapon aim component, if it has one
 
 
        protected List<WeaponModel> _weaponModels;

        /// <summary>
        /// Sets the weapon attachment
        /// </summary>
        protected override void PreInitialization()
        {
            base.PreInitialization();
            // filler if the WeaponAttachment has not been set
            if (WeaponAttachment == null)
            {
                WeaponAttachment = transform;
            }
        }

        // Initialization
        protected override void Initialization()
        {
            InstantiateWeapon(InitialWeapon);
   
            base.Initialization();
            Setup();
        }

        /// <summary>
        /// Grabs various components and inits stuff
        /// </summary>
        public virtual void Setup()
        {
            _character = this.gameObject.GetComponentInParent<Character>();
            _weaponModels = new List<WeaponModel>();
            foreach (WeaponModel model in _character.gameObject.GetComponentsInChildren<WeaponModel>())
            {
                _weaponModels.Add(model);
            }
            CharacterAnimator = _animator;
        }
 
        public override void ProcessAbility()
        {
            base.ProcessAbility();
            HandleFeedbacks(); 
        }

        /// <summary>
        /// Triggers the weapon used feedback if needed
        /// </summary>
        protected virtual void HandleFeedbacks()
        {
            if (CurrentWeapon != null)
            {
                if (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse)
                {
                    WeaponUseFeedback?.PlayFeedbacks();
                }
            }
        }
  
 
        public virtual void ShootStart()
        {
            PlayAbilityStartFeedbacks();
            CurrentWeapon.ShootWithWeapon();
        }

 
        public virtual void ShootStop()
        {
            if (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle)
            {
                return;
            }
 
            ForceStop();
        }
 
        public virtual void ForceStop()
        {
            StopStartFeedbacks();
            PlayAbilityStopFeedbacks();
            if (CurrentWeapon != null)
            {
                CurrentWeapon.TurnWeaponOff();
            }
        }
  
        protected virtual void InstantiateWeapon(Weapon newWeapon)
        {
            CurrentWeapon = InitialWeapon;
            CurrentWeapon.name = newWeapon.name;
            CurrentWeapon.transform.parent = WeaponAttachment.transform; 
 
            // we turn off the gun's emitters.
            CurrentWeapon.Initialization(); 
            CurrentWeapon.InitializeAnimatorParameters();
            InitializeAnimatorParameters();
        }

    

        protected override void OnRespawn()
        {
            base.OnRespawn();
            Setup();
        }
    }
}