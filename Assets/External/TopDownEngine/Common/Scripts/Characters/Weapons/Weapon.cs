using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This base class, meant to be extended (see ProjectileWeapon.cs for an example of that) handles rate of fire (rate of use actually), and ammo reloading
    /// </summary>
    [SelectionBase]
    public class Weapon : MMMonoBehaviour 
    {
 
 
        public enum WeaponStates { WeaponIdle, WeaponDelayBeforeUse, WeaponUse, WeaponIsBeingUsed }
 
        [MMReadOnly]
        [Tooltip("whether or not the weapon is currently active")]
        public bool WeaponCurrentlyActive = true;
 

        [MMInspectorGroup("Weapon Settings", true, 1)]
        public GameObject Owner;
        [MMReadOnly]
        public WeaponStates currentState;

        [Tooltip("how many times should shoot")]
        public int shootingCountPerWeaponUse = 5;

        [Tooltip("delay before starting shooting")]
        public float delayBeforeUse = 0.5f;
        private float delayBeforeUseCounter;

        [Tooltip("delay between two consecutive shootings")]
        public float delayBetweenTwoShootings = 0.5f; 

        [MMInspectorGroup("Animation Parameters Names", true, 2)]
        [Tooltip("the name of the weapon's in use animation parameter : true at each frame the weapon has started firing but hasn't stopped yet")]
        public string UseAnimationParameter;

        [MMInspectorGroup("Animation Parameters Names", true, 3)]
        [Tooltip("the feedback to play while the weapon is in use")]
        public MMFeedbacks WeaponUsedMMFeedback;
        public MMStateMachine<WeaponStates> WeaponState;
 
 
        protected float _movementMultiplierStorage = 1f;

        public float MovementMultiplierStorage
        {
            get => _movementMultiplierStorage;
            set => _movementMultiplierStorage = value;
        }
        protected Animator _ownerAnimator; 
 
        protected TopDownController _controller; 
 
        protected List<HashSet<int>> _animatorParameters;
        protected HashSet<int> _ownerAnimatorParameters;
 
        protected const string _aliveAnimationParameterName = "Alive";
        protected int _idleAnimationParameter;
        protected int _useAnimationParameter;

        private IEnumerator shootingCoroutine;

        /// <summary>
        /// On start we initialize our weapon
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }

        /// <summary>
        /// Initialize this weapon.
        /// </summary>
        public virtual void Initialization()
        {
            WeaponState = new MMStateMachine<WeaponStates>(gameObject, true);
            WeaponState.ChangeState(WeaponStates.WeaponIdle);
            _animatorParameters = new List<HashSet<int>>();
 
            InitializeAnimatorParameters();
            InitializeFeedbacks();       
        }

        protected virtual void InitializeFeedbacks()
        {
            WeaponUsedMMFeedback?.Initialization(this.gameObject);
        }
 
 
        public virtual void WeaponInputStart()
        {
             
        }


        public virtual void ShootWithWeapon()
		{
            if (WeaponState.CurrentState != WeaponStates.WeaponIdle)
            {
                return;
            }

            WeaponState.ChangeState(WeaponStates.WeaponDelayBeforeUse);
        }

        public virtual void TurnWeaponOff()
		{
            if (WeaponState.CurrentState != WeaponStates.WeaponIsBeingUsed)
            {
                return;
            }

            delayBeforeUseCounter = delayBeforeUse;
            StopCoroutine(shootingCoroutine);
            WeaponState.ChangeState(WeaponStates.WeaponIdle);
        }
  

 
        protected virtual void Update()
        {          
            UpdateAnimator();
        }

 
        protected virtual void LateUpdate()
        {
            ProcessWeaponState();
        }

 
        protected virtual void ProcessWeaponState()
        {
         
            if (WeaponState == null) { return; }
            
            currentState = WeaponState.CurrentState;

            switch (WeaponState.CurrentState)
            {
                case WeaponStates.WeaponIdle:
                    CaseWeaponIdle();
                    break;

                case WeaponStates.WeaponDelayBeforeUse:
                    CaseWeaponDelayBeforeUse();
                    break;

                case WeaponStates.WeaponUse:
                    CaseWeaponUse();
                    break;
                case WeaponStates.WeaponIsBeingUsed:
                    break;

            }
        }
        public virtual void CaseWeaponIdle()
        {

        }

        public virtual void CaseWeaponDelayBeforeUse()
        {
            delayBeforeUseCounter -= Time.deltaTime;
            if (delayBeforeUseCounter <= 0)
            {
                WeaponState.ChangeState(WeaponStates.WeaponUse);
            }
        }
 
        public virtual void CaseWeaponUse()
        {
            shootingCoroutine = WeaponUsedCoroutine();
            StartCoroutine(shootingCoroutine);
            WeaponState.ChangeState(WeaponStates.WeaponIsBeingUsed);
        }
  
        public virtual IEnumerator WeaponUsedCoroutine()
        {
            float interval = delayBetweenTwoShootings;
            int remainingShots = shootingCountPerWeaponUse;
 
            while (remainingShots > 0)
            {
                WeaponUse();
                remainingShots--;
                yield return MMCoroutine.WaitFor(interval);
            }
            WeaponState.ChangeState(WeaponStates.WeaponIdle);
        }
 
        public virtual void WeaponUse()
        {
            WeaponUsedMMFeedback?.PlayFeedbacks(this.transform.position);
        }
 
 
        public virtual void InitializeAnimatorParameters()
        {
            if (_ownerAnimator != null)
            {
                _ownerAnimatorParameters = new HashSet<int>();
                AddParametersToAnimator(_ownerAnimator, _ownerAnimatorParameters);
            }
        }

        protected virtual void AddParametersToAnimator(Animator animator, HashSet<int> list)
        { 
            MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, UseAnimationParameter, out _useAnimationParameter, AnimatorControllerParameterType.Bool, list);
        }

 
        public virtual void UpdateAnimator()
        {
 
            if ((_ownerAnimator != null) && (WeaponState != null) && (_ownerAnimatorParameters != null))
            {
                UpdateAnimator(_ownerAnimator, _ownerAnimatorParameters);
            }
        }

        protected virtual void UpdateAnimator(Animator animator, HashSet<int> list)
        {
            MMAnimatorExtensions.UpdateAnimatorBool(animator, _idleAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle), list);
            MMAnimatorExtensions.UpdateAnimatorBool(animator, _useAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse) || (WeaponState.CurrentState == Weapon.WeaponStates.WeaponIsBeingUsed), list);
        }
    }
}