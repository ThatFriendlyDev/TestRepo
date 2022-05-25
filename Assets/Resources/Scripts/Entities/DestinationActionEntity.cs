using System;
using UnityEngine;

public class DestinationActionEntity : MonoBehaviour
{
    [Header("Components")]
    public new Rigidbody rigidbody;
    public new Collider collider;

    [Header("Config")]
    public float minDistanceFromDestination = 2.5f;
    public float movementSpeed = 12f;
    public float rotationSpeed = 5f;

    [HideInInspector] public Transform destination;
    [HideInInspector] public Action callback;

    protected virtual void FixedUpdate()
    {
        if (!this.HasDestination())
        {
            return;
        }

        if (Vector3.Distance(this.transform.position, this.destination.transform.position) > this.minDistanceFromDestination)
        {
            return;
        }

        if (Vector3.Distance(this.transform.position, this.destination.position) <= 0.1f)
        {
            if (this.callback != null)
            {
                this.callback.Invoke();
            }

            Destroy(this.gameObject);
            return;
        }

        this.transform.SetPositionAndRotation(
            Vector3.MoveTowards(this.transform.position, this.destination.position, this.movementSpeed * Time.fixedDeltaTime),
            Quaternion.RotateTowards(this.transform.rotation, this.destination.rotation, this.rotationSpeed * Time.fixedDeltaTime)
        );
    }

    private bool HasDestination()
    {
        if (this.destination)
        {
            return true;
        }

        return false;
    }
}
