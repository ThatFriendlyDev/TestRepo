using MoreMountains.TopDownEngine;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;
    public CharacterMovement movement;

    private void Update()
    {
        this.animator.SetBool("Run", this.movement.IsMoving());
    }
}
