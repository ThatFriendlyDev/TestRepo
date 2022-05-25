﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this ability to a character, and it'll be able to rotate to face the movement's direction or the weapon's rotation
    /// </summary>
    [MMHiddenProperties("AbilityStartFeedbacks", "AbilityStopFeedbacks")]
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Orientation 3D")]
    public class CharacterOrientation3D : CharacterAbility
    {
        /// the possible rotation modes
		public enum RotationModes { None, MovementDirection}
        /// the possible rotation speeds
		public enum RotationSpeeds { Instant, Smooth, SmoothAbsolute }

        [Header("Rotation Mode")]

        /// whether the character should face movement direction, weapon direction, or both, or none
		[Tooltip("whether the character should face movement direction, weapon direction, or both, or none")]
        public RotationModes RotationMode = RotationModes.None;
        /// if this is false, no rotation will occur
        [Tooltip("if this is false, no rotation will occur")]
        public bool CharacterRotationAuthorized = true;

        [Header("Movement Direction")]

        /// If this is true, we'll rotate our model towards the direction
        [Tooltip("If this is true, we'll rotate our model towards the direction")]
        public bool ShouldRotateToFaceMovementDirection = true;
        /// the current rotation mode
        [MMCondition("ShouldRotateToFaceMovementDirection", true)]
        [Tooltip("the current rotation mode")]
        public RotationSpeeds MovementRotationSpeed = RotationSpeeds.Instant;
        /// the object we want to rotate towards direction. If left empty, we'll use the Character's model
        [MMCondition("ShouldRotateToFaceMovementDirection", true)]
        [Tooltip("the object we want to rotate towards direction. If left empty, we'll use the Character's model")]
        public GameObject MovementRotatingModel;
        /// the speed at which to rotate towards direction (smooth and absolute only)
        [MMCondition("ShouldRotateToFaceMovementDirection", true)]
        [Tooltip("the speed at which to rotate towards direction (smooth and absolute only)")]
        public float RotateToFaceMovementDirectionSpeed = 10f;
        /// the threshold after which we start rotating (absolute mode only)
        [MMCondition("ShouldRotateToFaceMovementDirection", true)]
        [Tooltip("the threshold after which we start rotating (absolute mode only)")]
        public float AbsoluteThresholdMovement = 0.5f;
        /// the direction of the model
        [MMReadOnly]
        [Tooltip("the direction of the model")]
        public Vector3 ModelDirection;
        /// the direction of the model in angle values
        [MMReadOnly]
        [Tooltip("the direction of the model in angle values")]
        public Vector3 ModelAngles;

        [Header("Weapon Direction")]

        /// If this is true, we'll rotate our model towards the weapon's direction
        [Tooltip("If this is true, we'll rotate our model towards the weapon's direction")]
        public bool ShouldRotateToFaceWeaponDirection = true;
        /// the current rotation mode
        [MMCondition("ShouldRotateToFaceWeaponDirection", true)]
        [Tooltip("the current rotation mode")]
        public RotationSpeeds WeaponRotationSpeed = RotationSpeeds.Instant;
        /// the object we want to rotate towards direction. If left empty, we'll use the Character's model
        [MMCondition("ShouldRotateToFaceWeaponDirection", true)]
        [Tooltip("the object we want to rotate towards direction. If left empty, we'll use the Character's model")]
        public GameObject WeaponRotatingModel;
        /// the speed at which to rotate towards direction (smooth and absolute only)
        [MMCondition("ShouldRotateToFaceWeaponDirection", true)]
        [Tooltip("the speed at which to rotate towards direction (smooth and absolute only)")]
        public float RotateToFaceWeaponDirectionSpeed = 10f;
        /// the threshold after which we start rotating (absolute mode only)
        [MMCondition("ShouldRotateToFaceWeaponDirection", true)]
        [Tooltip("the threshold after which we start rotating (absolute mode only)")]
        public float AbsoluteThresholdWeapon = 0.5f;
        /// the threshold after which we start rotating (absolute mode only)
        [MMCondition("ShouldRotateToFaceWeaponDirection", true)]
        [Tooltip("the threshold after which we start rotating (absolute mode only)")]
        public bool LockVerticalRotation = true;

        [Header("Animation")]

        /// the speed at which the instant rotation animation parameter float resets to 0
        [Tooltip("the speed at which the instant rotation animation parameter float resets to 0")]
        public float RotationSpeedResetSpeed = 2f;
        /// the speed at which the YRotationOffsetSmoothed should lerp
        [Tooltip("the speed at which the YRotationOffsetSmoothed should lerp")]
        public float RotationOffsetSmoothSpeed = 1f;

        [Header("Forced Rotation")]

        /// whether the character is being applied a forced rotation
        [Tooltip("whether the character is being applied a forced rotation")]
        public bool ForcedRotation = false;
        /// the forced rotation applied by an external script
        [MMCondition("ForcedRotation", true)]
        [Tooltip("the forced rotation applied by an external script")]
        public Vector3 ForcedRotationDirection;

        public Vector3 RelativeSpeed { get { return _relativeSpeed; } }
        public Vector3 RelativeSpeedNormalized { get { return _relativeSpeedNormalized; } }
        public float RotationSpeed { get { return _rotationSpeed; } }
        public Vector3 CurrentDirection { get { return _currentDirection; } }

        protected CharacterHandleWeapon _characterHandleWeapon;
 
        protected Vector3 _lastRegisteredVelocity;
        protected Vector3 _rotationDirection;
        protected Vector3 _lastMovement = Vector3.zero;
        protected Vector3 _lastAim = Vector3.zero;
        protected Vector3 _relativeSpeed;
        protected Vector3 _remappedSpeed = Vector3.zero;
        protected Vector3 _relativeMaximum;
        protected Vector3 _relativeSpeedNormalized;
        protected bool _secondaryMovementTriggered = false;
        protected Quaternion _tmpRotation;
        protected Quaternion _newMovementQuaternion;
        protected Quaternion _newWeaponQuaternion;
        protected bool _shouldRotateTowardsWeapon;
        protected float _rotationSpeed;
        protected float _modelAnglesYLastFrame;
        protected float _yRotationOffset;
        protected float _yRotationOffsetSmoothed;
        protected Vector3 _currentDirection;
        protected Vector3 _weaponRotationDirection;
 
 


        /// <summary>
        /// On init we grab our model if necessary
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();

            if ((_model == null) && (MovementRotatingModel == null) && (WeaponRotatingModel == null))
            {
                Debug.LogError("CharacterOrientation3D on "+this.name+" : you need to set a CharacterModel on your Character component, and/or specify MovementRotatingModel and WeaponRotatingModel on your CharacterOrientation3D inspector. Check the documentation to learn more about this.");
            }

            if (MovementRotatingModel == null)
            {
                MovementRotatingModel = _model;
            } 

            _characterHandleWeapon = _character?.FindAbility<CharacterHandleWeapon>();
            if (WeaponRotatingModel == null)
            {
                WeaponRotatingModel = _model;
            }
        }

        /// <summary>
        /// Every frame we rotate towards the direction
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();

            if ((MovementRotatingModel == null) && (WeaponRotatingModel == null))
            {
                return;
            }

            if (!AbilityAuthorized)
            {
                return;
            }

        
            if (CharacterRotationAuthorized)
            {
                RotateToFaceMovementDirection();
 
                RotateModel();
            }
        }


        protected virtual void FixedUpdate()
        {
            ComputeRelativeSpeeds();
        }


        /// <summary>
        /// Rotates the player model to face the current direction
        /// </summary>
        protected virtual void RotateToFaceMovementDirection()
        {
            // if we're not supposed to face our direction, we do nothing and exit
            if (!ShouldRotateToFaceMovementDirection) { return; }
            if ((RotationMode != RotationModes.MovementDirection)) { return; }

            _currentDirection = ForcedRotation ? ForcedRotationDirection : _controller.CurrentDirection;

            // if the rotation mode is instant, we simply rotate to face our direction
            if (MovementRotationSpeed == RotationSpeeds.Instant)
            {
                if (_currentDirection != Vector3.zero)
                {
                    _newMovementQuaternion = Quaternion.LookRotation(_currentDirection);
                }
            }

            // if the rotation mode is smooth, we lerp towards our direction
            if (MovementRotationSpeed == RotationSpeeds.Smooth)
            {
                if (_currentDirection != Vector3.zero)
                {
                    _tmpRotation = Quaternion.LookRotation(_currentDirection);
                    _newMovementQuaternion = Quaternion.Slerp(MovementRotatingModel.transform.rotation, _tmpRotation, Time.deltaTime * RotateToFaceMovementDirectionSpeed);
                }
            }

            // if the rotation mode is smooth, we lerp towards our direction even if the input has been released
            if (MovementRotationSpeed == RotationSpeeds.SmoothAbsolute)
            {
                if (_currentDirection.normalized.magnitude >= AbsoluteThresholdMovement)
                {
                    _lastMovement = _currentDirection;
                }
                if (_lastMovement != Vector3.zero)
                {
                    _tmpRotation = Quaternion.LookRotation(_lastMovement);
                    _newMovementQuaternion = Quaternion.Slerp(MovementRotatingModel.transform.rotation, _tmpRotation, Time.deltaTime * RotateToFaceMovementDirectionSpeed);
                }
            }
            
            ModelDirection = MovementRotatingModel.transform.forward.normalized;
            ModelAngles = MovementRotatingModel.transform.eulerAngles;
        }

       
        /// <summary>
        /// Rotates models if needed
        /// </summary>
        protected virtual void RotateModel()
        {
            MovementRotatingModel.transform.rotation = _newMovementQuaternion;

            if (_shouldRotateTowardsWeapon && (_weaponRotationDirection != Vector3.zero))
            {
                WeaponRotatingModel.transform.rotation = _newWeaponQuaternion;
            }
        }

        protected Vector3 _positionLastFrame;
        protected Vector3 _newSpeed;

        /// <summary>
        /// Computes the relative speeds
        /// </summary>
        protected virtual void ComputeRelativeSpeeds()
        {
            if ((MovementRotatingModel == null) && (WeaponRotatingModel == null))
            {
                return;
            }
            
            if (Time.deltaTime != 0f)
            {
                _newSpeed = (this.transform.position - _positionLastFrame) / Time.deltaTime;
            }

            // relative speed
            if ((_characterHandleWeapon == null) || (_characterHandleWeapon.CurrentWeapon == null))
            {
                _relativeSpeed = MovementRotatingModel.transform.InverseTransformVector(_newSpeed);
            }
            else
            {
                _relativeSpeed = WeaponRotatingModel.transform.InverseTransformVector(_newSpeed);
            }

            // remapped speed

            float maxSpeed = 0f;
            if (_characterMovement != null)
            {
                maxSpeed = _characterMovement.WalkSpeed;
            }
      
            _relativeMaximum = _model.transform.TransformVector(Vector3.one);
            _remappedSpeed.x = MMMaths.Remap(_relativeSpeed.x, 0f, maxSpeed, 0f, _relativeMaximum.x);
            _remappedSpeed.y = MMMaths.Remap(_relativeSpeed.y, 0f, maxSpeed, 0f, _relativeMaximum.y);
            _remappedSpeed.z = MMMaths.Remap(_relativeSpeed.z, 0f, maxSpeed, 0f, _relativeMaximum.z);
            
            // relative speed normalized
            _relativeSpeedNormalized = _relativeSpeed.normalized;
            _yRotationOffset = _modelAnglesYLastFrame - ModelAngles.y;

            _yRotationOffsetSmoothed = Mathf.Lerp(_yRotationOffsetSmoothed, _yRotationOffset, RotationOffsetSmoothSpeed * Time.deltaTime);
            
            // RotationSpeed
            if (Mathf.Abs(_modelAnglesYLastFrame - ModelAngles.y) > 1f)
            {
                _rotationSpeed = Mathf.Abs(_modelAnglesYLastFrame - ModelAngles.y);
            }
            else
            {
                _rotationSpeed -= Time.time * RotationSpeedResetSpeed;
            }
            if (_rotationSpeed <= 0f)
            {
                _rotationSpeed = 0f;
            }

            _modelAnglesYLastFrame = ModelAngles.y;
            _positionLastFrame = this.transform.position;
        }

        /// <summary>
        /// Forces the character's model to face in the specified direction
        /// </summary>
        /// <param name="direction"></param>
        public virtual void Face(Character.FacingDirections direction)
        {
            switch (direction)
            {
                case Character.FacingDirections.East:
                    _newMovementQuaternion = Quaternion.LookRotation(Vector3.right);
                    break;
                case Character.FacingDirections.North:
                    _newMovementQuaternion = Quaternion.LookRotation(Vector3.forward);
                    break;
                case Character.FacingDirections.South:
                    _newMovementQuaternion = Quaternion.LookRotation(Vector3.back);
                    break;
                case Character.FacingDirections.West:
                    _newMovementQuaternion = Quaternion.LookRotation(Vector3.left);
                    break;
            }
        }

        /// <summary>
        /// Adds required animator parameters to the animator parameters list if they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
        }

        /// <summary>
        /// Sends the current speed and the current value of the Walking state to the animator
        /// </summary>
        public override void UpdateAnimator()
        {
        }
    }
}