using MoreMountains.TopDownEngine;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public Projectile projectile;
    public bool canMultiply = true;

	public GameObject Owner { get => projectile.GetOwner(); set => projectile.SetOwner(value); }

	private void OnDisable()
	{
		canMultiply = true;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
	}
 
}
