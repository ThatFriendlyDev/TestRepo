using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine.AI;

public class AIActionPathfinderToTarget3D : AIAction
{
    public Transform target;
    public Transform aiMoveArea;

    [SerializeField]
    private Vector3 lastTargetPosition;

    [SerializeField]
    protected AIMovement _characterMovement;
    [SerializeField]
    protected AIPathFinder _characterPathfinder3D;

    /// <summary>
    /// On init we grab our CharacterMovement ability
    /// </summary>
    public override void Initialization()
    {
        if (_characterPathfinder3D == null)
        {
            Debug.LogWarning(this.name + " : the AIActionPathfinderToTarget3D AI Action requires the CharacterPathfinder3D ability");
        }
    }

    public override void OnEnterState()
    {
        var newTargetPosition = RandomPosition(Random.Range(3, 10));
        target.transform.position = newTargetPosition;

        lastTargetPosition = newTargetPosition;
        _characterPathfinder3D.SetNewDestination(target);
        base.OnEnterState();
    }
 

    public Vector3 RandomPosition(float radius)
    {
        var randDirection = Random.insideUnitSphere * radius;
        randDirection += _characterPathfinder3D.transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, radius, 1);
        return navHit.position;
    }

    /// <summary>
    /// On PerformAction we move
    /// </summary>
    public override void PerformAction()
    {
        Move();
    }

    /// <summary>
    /// Moves the character towards the target if needed
    /// </summary>
    protected virtual void Move()
    {
        if (_brain.Target == null)
        {
         
            return;
        }
        else
        {
             
        }
    }

    /// <summary>
    /// On exit state we stop our movement
    /// </summary>
    public override void OnExitState()
    {
        base.OnExitState();

        _characterPathfinder3D?.SetNewDestination(null);
        _characterMovement?.SetMovement(Vector2.zero);
    }
}