using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Player player;

    private void Update()
    {
        this.animator.SetBool("Run", this.player.state == EPlayerState.Run);
    }
}
