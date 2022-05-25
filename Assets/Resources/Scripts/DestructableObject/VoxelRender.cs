using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRender : SerializedMonoBehaviour
{
    public bool _hasSpawnedVoxels;
    public float _explosionRadius;
    private float _destructionRadius;
    private float _destructionForceMin;
    private float _destructionForceMax;
    [HideInInspector]
    public float _voxelScale;

    [Header("Dependencies")]
    [SerializeField]
    private VoxelDynamicColliderGenerator _voxelColliderGenerator;
    [SerializeField]
    private GameObject _voxelPrefab;
    [SerializeField]
    private MMSimpleObjectPooler _voxelSpawner;
    private MeshRenderer _meshRenderer;

    Mesh _mesh;
    private Vector3 _meshScale;
    List<Vector3> _vertices;
    List<int> _triangles;

    private int[,,] _voxelsData;
    private VoxelData[] _voxelArray;
    private DestructableObject _destructableObject;
    private int voxelCount;
    private List<Rigidbody> _spawnedVoxels;
    [SerializeField]
    private Transform _instancesContainer;

    private void Awake()
    {

        this._mesh = GetComponent<MeshFilter>().mesh;
        this._meshRenderer = GetComponent<MeshRenderer>();
        this._spawnedVoxels = new List<Rigidbody>();
        
    }

	private void Start()
	{
        PopulateWithVoxels();
    }

	public void Initialize(int[,,] data, float voxelScale, Vector3 meshScale, float destructionRadius, float destructionForceMin, float destructionForceMax, DestructableObject destructableObject)
    {
        this._mesh = GetComponent<MeshFilter>().mesh;
        this._voxelsData = data;
        this._meshScale = meshScale / voxelScale;
        voxelCount = (int)(Mathf.Ceil(this._meshScale.x) * Mathf.Ceil(this._meshScale.y) * Mathf.Ceil(this._meshScale.z));
        this._voxelArray = new VoxelData[voxelCount];
        _voxelScale = voxelScale;
        this._destructionRadius = destructionRadius;
        this._destructionForceMin = destructionForceMin;
        this._destructionForceMax = destructionForceMax;
        this._destructableObject = destructableObject;
        this.GenerateVoxelMesh();

        this.UpdateMesh();
        this.transform.localScale = new Vector3(1 / meshScale.x, 1 / meshScale.y, 1 / meshScale.z);
        this.transform.localPosition = new Vector3(-0.5f + (voxelScale / 2) / meshScale.x, -0.5f + (voxelScale / 2) / meshScale.y, -0.5f + (voxelScale / 2) / meshScale.z);
    }

    void GenerateVoxelMesh()
    {
        this._vertices = new List<Vector3>();
        this._triangles = new List<int>();
        int voxelIndex = 0;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int z = 0; z < this.Depth; z++)
                {
                    Vector3 voxelLocalPos = new Vector3(x * this._voxelScale, y * this._voxelScale, z * this._voxelScale);
                    VoxelData newVoxel = new VoxelData(voxelIndex, GetVoxelRaw(x, y, z), x, y, z, voxelLocalPos, this._voxelScale * 0.5f);
                    this._voxelArray[voxelIndex] = newVoxel;

                    voxelIndex++;
                    if (newVoxel._isUsed == 0)
                    {
                        continue;
                    }

                    MakeCube(newVoxel);
                }
            }
        }
      //  this._voxelColliderGenerator.Create();
    }

    public uint GetVoxelIndexFromPosition(VoxelData voxel)
    {
        return GetVoxelIndexFromPosition(voxel.LocalPosition().x, voxel.LocalPosition().y, voxel.LocalPosition().z);
    }
    public uint GetVoxelIndexFromPosition(float posX, float posY, float posZ)
    {
        uint voxelIndex = (uint)((((posX + 1) * Height - (Height - (posY + 1))) * Depth - (Depth - (posZ + 1)) - 1) / (this._voxelScale));
        return (uint)(voxelIndex);

    }

    void MakeCube(VoxelData voxel)
    {

        for (int i = 0; i < 6; i++)
        {
            if (this.GetNeighbor(voxel, (Direction)i) == 0)
            {
                MakeFace((Direction)i, voxel);
            }
        }
    }

    void MakeFace(Direction dir, VoxelData voxel)
    {
        this._vertices.AddRange(CubeMeshData.FaceVertices(dir, voxel.LocalScale(), voxel.LocalPosition()));
        int vertCount = this._vertices.Count;
        this._triangles.Add(vertCount - 4);
        this._triangles.Add(vertCount - 4 + 1);
        this._triangles.Add(vertCount - 4 + 2);
        this._triangles.Add(vertCount - 4);
        this._triangles.Add(vertCount - 4 + 2);
        this._triangles.Add(vertCount - 4 + 3);
    }


    public int GetVoxelRaw(int x, int y, int z)
    {
        return this._voxelsData[x, y, z];
    }

    public int GetVoxelByLocalPos(Vector3 localCoordinates)
    {
        return 0;
    }

    public int GetNeighbor(VoxelData voxelIndexes, Direction dir)
    {
        DataCoordinate offsetToCheck = this._coordinateOffset[(int)dir];
        DataCoordinate neighborCoordinate = new DataCoordinate(voxelIndexes.xIndex + offsetToCheck.x, voxelIndexes.yIndex + offsetToCheck.y, voxelIndexes.zIndex + offsetToCheck.z);
        if (neighborCoordinate.x < 0 || neighborCoordinate.x >= Width || neighborCoordinate.y < 0 || neighborCoordinate.y >= Height || neighborCoordinate.z < 0 || neighborCoordinate.z >= Depth)
        {
            return 0;
        }
        else
        {
            return this.GetVoxelRaw(neighborCoordinate.x, neighborCoordinate.y, neighborCoordinate.z);
        }
    }

    Vector3 _contactPosition = Vector3.zero;


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_contactPosition, 0.1f * Vector3.one);
    }


    public void PopulateWithVoxels()
    {
 
        this._hasSpawnedVoxels = true;
      
        for (int i = 0; i < this._voxelArray.Length; i++)
        {

            var pooledVoxel = this._voxelSpawner.GetPooledGameObject();
            pooledVoxel.transform.position = transform.TransformPoint(_voxelArray[i].LocalPosition());
            pooledVoxel.transform.rotation = this._destructableObject.transform.rotation;
            pooledVoxel.transform.localScale = this._voxelScale * Vector3.one * 0.9f;
            pooledVoxel.gameObject.SetActive(true);

             this._voxelsData[_voxelArray[i].xIndex, _voxelArray[i].yIndex, _voxelArray[i].zIndex] = 0;
            Rigidbody rb = pooledVoxel.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            _spawnedVoxels.Add(rb); 
        }
 
        this.GenerateVoxelMesh();
        this.UpdateMesh();
    }

    public void ReleaseSpawnedVoxels()
    {
        foreach (Rigidbody spawnedVoxel in this._spawnedVoxels)
            spawnedVoxel.gameObject.SetActive(false);

        this._spawnedVoxels.Clear();
    }

	public void OnCollisionWithPlayer(float minDestructionForce, float maxDestructionForce, float radius)
	{
        StartCoroutine(Explode(minDestructionForce, maxDestructionForce, radius));
    }

    public IEnumerator Explode(float minDestructionForce, float maxDestructionForce, float radius)
	{
       

        for (int i = 0; i < this._spawnedVoxels.Count; i++)
        {
         //   float distance = Vector3.Distance(this._spawnedVoxels[i].position, playerController.transform.position);
           // _spawnedVoxels[i].AddForce(playerController._ballMotor.GetForwardDirection() * 100);
          //  _spawnedVoxels[i].AddExplosionForce(10 * 10 * Mathf.Min(50, 1 / Mathf.Pow(Vector3.Distance(this._spawnedVoxels[i].position, playerController.transform.position), 4)) * Random.Range(minDestructionForce, maxDestructionForce), playerController.transform.position - transform.forward, radius);
        }
        this._instancesContainer.gameObject.SetActive(false);
        yield return null;
    }


    public void Explode(Transform shootingBall, float minDestructionForce, float maxDestructionForce, float radius)
    {

        for (int i = 0; i < this._spawnedVoxels.Count; i++)
        {
            _spawnedVoxels[i].AddExplosionForce(10 * 10 * Mathf.Min(50, 1 / Mathf.Pow(Vector3.Distance(this._spawnedVoxels[i].position, shootingBall.transform.position), 4)) * Random.Range(minDestructionForce, maxDestructionForce), shootingBall.transform.position - transform.forward, radius);
        }
        this._instancesContainer.gameObject.SetActive(false);
    }


    public void OnCollisionWithShootingBall(Vector3 shootingBallPosition)
	{
        for (int i = 0; i < this._spawnedVoxels.Count; i++)
        {
            _spawnedVoxels[i].useGravity = false;
           // _spawnedVoxels[i].AddExplosionForce(10 * Mathf.Min(50, 1 / Mathf.Pow(Vector3.Distance(this._spawnedVoxels[i].position, shootingBallPosition), 4)) * Random.Range(150, 250), shootingBallPosition, 0.25f);
        }
    }

	Vector2[] uvs;

    void UpdateMesh()
    {
        this._mesh.Clear();
        this._mesh.vertices = this._vertices.ToArray();
        this._mesh.triangles = this._triangles.ToArray();

        int vertCount = this._vertices.Count;


        this._mesh.RecalculateNormals();
        this._mesh.RecalculateTangents();
        this._mesh.Optimize();

    }

 

	struct DataCoordinate
    {
        public int x;
        public int y;
        public int z;

        public DataCoordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public VoxelData[] GetVoxelArray()
    {
        return this._voxelArray;
    }


    public int Width
    {
        get { return (int)Mathf.Ceil(this._meshScale.x); }
    }

    public int Height
    {
        get { return (int)Mathf.Ceil(this._meshScale.y); }
    }

    public int Depth
    {
        get { return (int)Mathf.Ceil(this._meshScale.z); }
    }


    DataCoordinate[] _coordinateOffset =
    {
        new DataCoordinate(0, 0, 1),
        new DataCoordinate(1, 0, 0),
        new DataCoordinate(0, 0 , -1),
        new DataCoordinate(-1, 0, 0),
        new DataCoordinate(0, 1, 0),
        new DataCoordinate(0, -1, 0)

    };

    public MeshRenderer GetMeshRenderer()
    {
        return this._meshRenderer;
    }
}

public enum Direction
{
    Forward,
    Right,
    Backward,
    Left,
    Up,
    Down
}

public struct UIntVec3
{
    public uint x;
    public uint y;
    public uint z;

    public UIntVec3(uint x, uint y, uint z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //Allows the vector to be converted to the format that Unity3D allows.
    public static explicit operator Vector3(UIntVec3 vec)
    {
        return new Vector3(vec.x, vec.y, vec.z);
    }
}

