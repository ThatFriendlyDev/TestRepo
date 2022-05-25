using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelInstanceCtrl : MonoBehaviour
{
    
    public void DestroyInstance()
	{
		gameObject.SetActive(false);
	}
}
