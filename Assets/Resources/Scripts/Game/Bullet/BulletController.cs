using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[SerializeField]
	private BouncyProjectile bouncyProjectile;
    private Rigidbody bulletRigidbody;

	private void Awake()
	{
        bulletRigidbody = transform.GetComponent<Rigidbody>();
	}

	void Start()
    {
        
    }
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(Tags.FieldSplitter))
		{
			bulletRigidbody.useGravity = true;
			bulletRigidbody.isKinematic = false;
			bouncyProjectile.CanBounce = true;
		}
	}
}
