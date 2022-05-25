using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public AIController aiController;

    [Tooltip("how fast the AI gains speed")]
    public float accelerationAmount;
    [Tooltip("how fast the AI loses speed")]
    public float deccelerationAmount;

    [Tooltip("maximum movement speed the AI will reach")]
    public float movementSpeed;

    [Tooltip("below this speed the AI will stop moving")]
    public float movementIdleTreshold;

    public CharacterController characterController;

    private float horizontalMovement;
    private float verticalMovement;
    private float currentAcceleration;

    private Vector2 movementInput;
    private Vector2 lerpedMovement;
    private Vector3 movement;


    protected const string speedAnimationParameterName = "Speed";
    protected const string idleAnimationParameterName = "Idle";
    protected int speedAnimationParameter;
    protected int idleAnimationParameter;



    // Start is called before the first frame update
    void Start()
    {
        aiController.SetMovementState(AIController.AIMovementState.Idle);
    }
 
    // Update is called once per frame
    void Update()
    {

        movementInput.x = horizontalMovement;
        movementInput.y = verticalMovement;
        Vector2 normalizedMovement = movementInput.normalized;
   

        if (normalizedMovement.magnitude == 0)
        {
            lerpedMovement = Vector2.Lerp(lerpedMovement, Vector2.zero, Time.deltaTime * deccelerationAmount);
        }
        else
        {
            lerpedMovement = Vector2.Lerp(lerpedMovement, Vector2.ClampMagnitude(normalizedMovement, movementSpeed), Time.deltaTime * accelerationAmount);
        }

        movement.x = lerpedMovement.x;
        movement.y = 0f;
        movement.z = lerpedMovement.y;

        movement *= movementSpeed;
        movement = Vector3.ClampMagnitude(movement, movementSpeed);

        if (movement.magnitude < movementIdleTreshold)
		{
            aiController.SetMovementState(AIController.AIMovementState.Idle);
            movement = Vector3.zero;
		} else
		{
            aiController.SetMovementState(AIController.AIMovementState.Moving);
        }

        characterController.Move(movement * Time.deltaTime);

    }


    public void SetMovement(Vector2 movement)
	{
        horizontalMovement = movement.x;
        verticalMovement = movement.y;
    }
}
