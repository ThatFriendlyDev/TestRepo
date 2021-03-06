using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathFinder : MonoBehaviour
{
    [Tooltip("the target the character should pathfind to")]
    public Transform Target;
    [Tooltip("the distance to waypoint at which the movement is considered complete")]
    public float DistanceToWaypointThreshold = 0.25f;
    [Tooltip("if the target point can't be reached, the distance threshold around that point in which to look for an alternative end point")]
    public float ClosestPointThreshold = 3f;

    [SerializeField]
    private AIMovement aIMovement;

    [MMReadOnly]
    [Tooltip("the current path")]
    public NavMeshPath AgentPath;

    [MMReadOnly]
    [Tooltip("a list of waypoints the character will go through")]
    public Vector3[] Waypoints;

    [MMReadOnly]
    [Tooltip("the index of the next waypoint")]
    public int NextWaypointIndex;

    [MMReadOnly]
    [Tooltip("the direction of the next waypoint")]
    public Vector3 NextWaypointDirection;

    [MMReadOnly]
    [Tooltip("the distance to the next waypoint")]
    public float DistanceToNextWaypoint;

    public event System.Action<int, int, float> OnPathProgress;

    protected int _waypoints;
    protected Vector3 _direction;
    protected Vector2 _newMovement;
    protected Vector3 _lastValidTargetPosition;
    protected Vector3 _closestNavmeshPosition;
    protected NavMeshHit _navMeshHit;
    protected bool _pathFound;

    void Awake()
    {
        AgentPath = new NavMeshPath();
        _lastValidTargetPosition = this.transform.position;
        Array.Resize(ref Waypoints, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }

        DetermineNextWaypoint();
        DetermineDistanceToNextWaypoint();
        MoveController();
    }

    /// <summary>
    /// Sets a new destination the character will pathfind to
    /// </summary>
    /// <param name="destinationTransform"></param>
    public virtual void SetNewDestination(Transform destinationTransform)
    {
        if (destinationTransform == null)
        {
            Target = null;
            return;
        }
        Target = destinationTransform;
        DeterminePath(this.transform.position, destinationTransform.position);
    }

    protected virtual void DeterminePath(Vector3 startingPosition, Vector3 targetPosition)
    {
        NextWaypointIndex = 0;

        _closestNavmeshPosition = targetPosition;
        if (NavMesh.SamplePosition(targetPosition, out _navMeshHit, ClosestPointThreshold, NavMesh.AllAreas))
        {
            _closestNavmeshPosition = _navMeshHit.position;
        }

        _pathFound = NavMesh.CalculatePath(startingPosition, _closestNavmeshPosition, NavMesh.AllAreas, AgentPath);
        if (_pathFound)
        {
            _lastValidTargetPosition = _closestNavmeshPosition;
        }
        else
        {
            NavMesh.CalculatePath(startingPosition, _lastValidTargetPosition, NavMesh.AllAreas, AgentPath);
        }

        // Waypoints = AgentPath.corners;
        _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
        if (_waypoints >= Waypoints.Length)
        {
            Array.Resize(ref Waypoints, _waypoints + 5);
            _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
        }
        if (_waypoints >= 2)
        {
            NextWaypointIndex = 1;
        }

        OnPathProgress?.Invoke(NextWaypointIndex, Waypoints.Length, Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]));
    }

    protected virtual void DetermineNextWaypoint()
    {
        if (_waypoints <= 0)
        {
            return;
        }
        if (NextWaypointIndex < 0)
        {
            return;
        }

        var distance = Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]);
        if (distance <= DistanceToWaypointThreshold)
        {
            if (NextWaypointIndex + 1 < _waypoints)
            {
                NextWaypointIndex++;
            }
            else
            {
                NextWaypointIndex = -1;
            }
            OnPathProgress?.Invoke(NextWaypointIndex, _waypoints, distance);
        }
    }

    protected virtual void DetermineDistanceToNextWaypoint()
    {
        if (NextWaypointIndex <= 0)
        {
            DistanceToNextWaypoint = 0;
        }
        else
        {
            DistanceToNextWaypoint = Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]);
        }
    }

    protected virtual void MoveController()
    {
        if ((Target == null) || (NextWaypointIndex <= 0))
        {
            aIMovement.SetMovement(Vector2.zero);
            return;
        }
        else
        {
            _direction = (Waypoints[NextWaypointIndex] - this.transform.position).normalized;
            _newMovement.x = _direction.x;
            _newMovement.y = _direction.z;
            aIMovement.SetMovement(_newMovement);
        }
    }
}
