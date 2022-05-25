using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Player player;

    [Header("Movement")]
    public float movementSpeed = 3f;

    [HideInInspector]
    public Vector3 inputDirection;

    private void FixedUpdate()
    {
        if (!GameManager.singleton.IsInProgress())
        {
            return;
        }

        this.inputDirection = ControllerManager.input.GetDirection();

        if (!this.IsMoving())
        {
            this.player.state = EPlayerState.Idle;
            return;
        }

        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.inputDirection), .15f);
        this.transform.Translate(this.movementSpeed * this.inputDirection * Time.fixedDeltaTime, Space.World);

        this.player.state = EPlayerState.Run;
    }

    public bool IsMoving()
    {
        return this.inputDirection != Vector3.zero;
    }
}
