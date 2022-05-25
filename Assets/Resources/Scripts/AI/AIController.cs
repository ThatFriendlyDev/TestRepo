using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
	public enum AIMovementState { Idle, Moving}
	public AIMovementState currentAIMovementState = AIMovementState.Idle;
	public enum AIConditionState { None, Shooting }
	public AIConditionState currentAIConditionState = AIConditionState.None;
 
	public Animator animator;
	public AIBrain aiBrain;

	private void Awake()
	{
		aiBrain.Owner = this.gameObject;
	}

	public void SetMovementState(AIMovementState newState)
	{
		currentAIMovementState = newState;
	}

	public void SetConditionState(AIConditionState newState)
	{
		currentAIConditionState = newState;
	}

}
