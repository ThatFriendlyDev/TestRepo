using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMeshCollider : MonoBehaviour
{
 
    public LayerMask _playerVoxelColliderLayer;
    public VoxelDestruction _voxelDestruction;
 
	public void ReplaceWithVoxels(Vector3 collisionPoint)
	{
		this._voxelDestruction.ReplaceMeshWithVoxels(collisionPoint);
	}

}
