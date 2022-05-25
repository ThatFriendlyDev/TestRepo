using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{
    public int _isUsed;
    public int Index { get; }
    public int xIndex { get; }
    public int yIndex { get; }
    public int zIndex { get; }
 
    private Vector3 _localPosition { get; }
    private float _localScale { get; }
    public VoxelData(int voxelIndex, int contributeToMesh, int x, int y, int z, Vector3 localPos, float scale)
    {
        this.Index = voxelIndex;
        this._isUsed = contributeToMesh;
        this.xIndex = x;
        this.yIndex = y;
        this.zIndex = z;
 
        this._localPosition = localPos;
        this._localScale = scale;
    }

 

    public Vector3 LocalPosition() { return this._localPosition; }

    public float LocalScale() { return this._localScale; }
}
