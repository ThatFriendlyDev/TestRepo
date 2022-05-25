using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public bool isEnemy;
    [SerializeField]
    private LayerMask _destroyerLayer;

 
    [SerializeField]
    private VoxelRender _voxelRender;
    [SerializeField]
    private float _meshVoxelSize = 0.5f;
    [SerializeField]
    private float _destructionRadius;
    [SerializeField]
    private float _destructionForceMin;
    [SerializeField]
    private float _destructionForceMax;

    [HideInInspector]
    public bool _isDestroyed;

  
    private int[,,] _data;
    private MeshRenderer _meshRenderer;
 

    private void Awake()
	{
  
        this._meshRenderer = this.transform.GetComponent<MeshRenderer>();
 
        this.CalcVoxelSpawnIndexes();
        this._voxelRender.Initialize(this._data, this._meshVoxelSize, transform.localScale, this._destructionRadius, this._destructionForceMin, this._destructionForceMax, this);
 
	}

	private void Start()
    {
        this._voxelRender.GetMeshRenderer().enabled = false;
         
        this._voxelRender.GetMeshRenderer().enabled = true;
    }
 
	private void CalcVoxelSpawnIndexes()
	{
        this._data = new int[_meshVoxelSize > 1 ? (int)transform.localScale.x : Mathf.RoundToInt(transform.localScale.x / _meshVoxelSize), _meshVoxelSize > 1 ? (int)transform.localScale.y : Mathf.RoundToInt(transform.localScale.y / _meshVoxelSize), _meshVoxelSize > 1 ? (int)transform.localScale.z : Mathf.RoundToInt(transform.localScale.z / _meshVoxelSize)];
        for (int x = 0; x < this._data.GetLength(0); x++)
        {
            for (int y = 0; y < this._data.GetLength(1); y++)
            {
                for (int z = 0; z < this._data.GetLength(2); z++)
                {
                    this._data[x, y, z] = 1;
                }
            }
        }
    }

    public void OnShootingBallCollision(Transform shootingBall)
	{
        StartCoroutine(FractureDestructableObject(shootingBall));
    }

	private void OnTriggerEnter(Collider other)
	{
 
        bool hasCollidedWithDestroyer = 1 << other.transform.gameObject.layer == this._destroyerLayer.value;
        if (hasCollidedWithDestroyer)
		{
            if (!_isDestroyed)
                StartCoroutine(FractureDestructableObject(null));
            //   BallController playerCtrl = other.GetComponent<BallController>();

            //  else if (_wallHP <= 0)
            //   this._voxelRender.OnCollisionWithPlayer(playerCtrl, _destructionForceMin, _destructionForceMax, _destructionRadius);
        }
	}
 
   

    public IEnumerator FractureDestructableObject(Transform shootingBall)
	{
        if (this._isDestroyed)
            yield break;

        
        if (this != null && this.transform.gameObject.activeSelf)
        {
            if (this._voxelRender.gameObject.activeSelf && !this._voxelRender._hasSpawnedVoxels)
            {

              //  yield return StartCoroutine(this._voxelRender.PopulateWithVoxels(shootingBall, _destructionForceMin, _destructionForceMax, isEnemy));
                this._voxelRender.GetMeshRenderer().enabled = true;
                this._meshRenderer.enabled = false;
            }
            this._isDestroyed = true;
        }
    }
 

    public void ActivateVoxelMesh()
	{

        this._voxelRender.GetMeshRenderer().enabled = true;
        this._meshRenderer.enabled = false;
    }

    
    public bool ShouldPlayerBounceOnShootingBallHit()
	{
 

        return _isDestroyed;
    }


}
