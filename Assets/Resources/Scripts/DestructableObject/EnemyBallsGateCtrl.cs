using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallsGateCtrl : MonoBehaviour
{
    [SerializeField]
    private Transform gateHolderLeft;
    [SerializeField]
    private Transform gateHolderRight;
    
    public void OpenGates()
	{

	}
    
    private IEnumerator OpenGatesCoroutines()
	{
        yield return null;
	}
}
