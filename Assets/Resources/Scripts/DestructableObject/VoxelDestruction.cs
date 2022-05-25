using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDestruction : MonoBehaviour
{

    public VoxelRender _voxelRenderer;
 
    private Vector3? _collisionPoint = null;
 
 
	private void OnDrawGizmos()
	{
        if (this._collisionPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(this._collisionPoint.Value, Vector3.one * 0.1f);
    }
    
    public void ReplaceMeshWithVoxels(Vector3 collisionPoint)
	{
     //   this._collisionPoint = collisionPoint;
 
    //    StartCoroutine(this._voxelRenderer.DeactivateCubes(this._collisionPoint.Value));
    }
    public void DestroyVoxels(Vector3 collisionPoint)
    {
//        this._collisionPoint = collisionPoint;

  //      StartCoroutine(this._voxelRenderer.DeactivateCubes(this._collisionPoint.Value));
    }


}
