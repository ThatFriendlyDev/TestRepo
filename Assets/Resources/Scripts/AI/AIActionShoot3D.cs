using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionShoot3D : AIAction
{
    public enum AimOrigins { Transform, SpawnPosition }

    [Header("Behaviour")]
    /// if true the Character will aim at the target when shooting
    [Tooltip("if true the Character will aim at the target when shooting")]
    public bool AimAtTarget = true;
    /// the point to consider as the aim origin
    [Tooltip("the point to consider as the aim origin")]
    public AimOrigins AimOrigin = AimOrigins.Transform;
    /// an offset to apply to the aim (useful to aim at the head/torso/etc automatically)
    [Tooltip("an offset to apply to the aim (useful to aim at the head/torso/etc automatically)")]
    public Vector3 ShootOffset;
    /// if this is set to true, vertical aim will be locked to remain horizontal
    [Tooltip("if this is set to true, vertical aim will be locked to remain horizontal")]
    public bool LockVerticalAim = false;

    [SerializeField]
    protected ProjectileWeapon _projectileWeapon;
    protected Vector3 _weaponAimDirection;
    protected int _numberOfShoots = 0;
    protected bool _shooting = false;


    /// <summary>
    /// On init we grab our CharacterHandleWeapon ability
    /// </summary>
    public override void Initialization()
    {

    }

    /// <summary>
    /// On PerformAction we face and aim if needed, and we shoot
    /// </summary>
    public override void PerformAction()
    {
        Shoot();
    }





    protected virtual void Shoot()
    {
        if (_numberOfShoots < 1)
        {

            _projectileWeapon.ShootWithWeapon();
            _numberOfShoots++;
        }
 
    }

    /// <summary>
    /// When entering the state we reset our shoot counter and grab our weapon
    /// </summary>
    public override void OnEnterState()
    {
        base.OnEnterState();
        _numberOfShoots = 0;
        _shooting = true;
    }

    /// <summary>
    /// When exiting the state we make sure we're not shooting anymore
    /// </summary>
    public override void OnExitState()
    {
        base.OnExitState();
        _shooting = false;
    }
}