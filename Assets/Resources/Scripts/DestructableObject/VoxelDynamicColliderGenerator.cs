using UnityEngine;
using System.Collections.Generic;

//Basic struct for storing 3 unsigned integers.
 

public class VoxelDynamicColliderGenerator : MonoBehaviour
{
    [SerializeField]
    private VoxelRender _voxelRender;
    [SerializeField]
    private VoxelDestruction _voxelDestruction;
 
 
    private List<BoxCollider> m_colliders = new List<BoxCollider>();

    public void Create()
    {
 
        GenerateMesh();       //Generates and attaches the mesh.
    }

    //Returns the position of a voxel in the array from its 3D co-ordinates.
    private static float GetVoxelDataIndex(float x, float y, float z)
    {
        //    return x | y << VOXEL_Y_SHIFT | z << VOXEL_Z_SHIFT;
        return 0;
    }

    //Returns the position of a voxel in 3D co-ordinates from its index in the array.
    private static Vector3 GetVoxelDataPosition(float index)
    {
        /*
        float blockX = index & 0xF;
        float blockY = (index >> VOXEL_Y_SHIFT) & 0xF;
        float blockZ = (index >> VOXEL_Z_SHIFT) & 0xF;
        return new Vector3(blockX, blockY, blockZ);*/
        return new Vector3();
    }

 

    //Generates the collision boxes for the mesh and applies them.
    public void GenerateMesh()
    {
        //Keeps track of whether a voxel has been checked.
        bool[] tested = new bool[_voxelRender.Width * this._voxelRender.Height * this._voxelRender.Depth];
        Dictionary<Vector3, Vector3> boxes = new Dictionary<Vector3, Vector3>();
        for (uint index = 0; index < tested.Length; ++index)
        {
            if (!tested[index])
            {
                tested[index] = true;
                if (this._voxelRender.GetVoxelArray()[index]._isUsed > 0)  //If the voxel contributes to the collision mesh.
                {
                    Vector3 voxelLocalPos = this._voxelRender.GetVoxelArray()[index].LocalPosition();
                    Vector3 boxStart = new Vector3((float)voxelLocalPos.x, (float)voxelLocalPos.y, (float)voxelLocalPos.z);
                     

                    Vector3 boxSize = new Vector3(1, 1, 1) * this._voxelRender._voxelScale;
                    bool canSpreadX = true;
                    bool canSpreadY = true;
                    bool canSpreadZ = true;
                    //Attempts to expand in all directions and stops in each direction when it no longer can.
                    while (canSpreadX || canSpreadY || canSpreadZ)
                    {
                        canSpreadX = TrySpreadX(canSpreadX, ref tested, boxStart, ref boxSize);
                        canSpreadY = TrySpreadY(canSpreadY, ref tested, boxStart, ref boxSize);
                        canSpreadZ = TrySpreadZ(canSpreadZ, ref tested, boxStart, ref boxSize);
                    }
                        boxes.Add(boxStart, boxSize);
                }
            }
        }
        SetCollisionMesh(boxes);    //Applies the collision boxes.
    }

    //Returns whether the box can continue to spread along the positive X axis.
    private bool TrySpreadX(bool canSpreadX, ref bool[] tested, Vector3 boxStart, ref Vector3 boxSize)
    {
        //Checks the square made by the Y and Z size on the X index one larger than the size of the
        //box.
        float yLimit = boxStart.y + boxSize.y;
        float zLimit = boxStart.z + boxSize.z;
        for (float y = boxStart.y; y < yLimit && canSpreadX; y += this._voxelRender._voxelScale)
        {
            for (float z = boxStart.z; z < zLimit; z += this._voxelRender._voxelScale) 
            {
                float newX = boxStart.x + boxSize.x;
                uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(newX, y, z);
                if (newX >= this._voxelRender.Width * this._voxelRender._voxelScale || tested[newIndex] || this._voxelRender.GetVoxelArray()[newIndex]._isUsed == 0)
                {
                    canSpreadX = false;
                }
            }
        }
        //If the box can spread, mark it as tested and increase the box size in the X dimension.
        if (canSpreadX)
        {
            for (float y = boxStart.y; y < yLimit; y += this._voxelRender._voxelScale)
            {
                for (float z = boxStart.z; z < zLimit; z += this._voxelRender._voxelScale)
                {
                    float newX = boxStart.x + boxSize.x;
                    uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(newX, y, z);
                    tested[newIndex] = true;
                }
            }
            boxSize.x += this._voxelRender._voxelScale;
        }
        return canSpreadX;
    }

    //Returns whether the box can continue to spread along the positive Y axis.
    private bool TrySpreadY(bool canSpreadY, ref bool[] tested, Vector3 boxStart, ref Vector3 boxSize)
    {
        //Checks the square made by the X and Z size on the Y index one larger than the size of the
        //box.
        float xLimit = boxStart.x + boxSize.x;
        float zLimit = boxStart.z + boxSize.z;
        for (float x = boxStart.x; x < xLimit && canSpreadY; x += this._voxelRender._voxelScale)
        {
            for (float z = boxStart.z; z < zLimit; z +=  this._voxelRender._voxelScale)
            {
                float newY = boxStart.y + boxSize.y;
                uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(x, newY, z);
                if (newY >= this._voxelRender.Height * this._voxelRender._voxelScale || tested[newIndex] || this._voxelRender.GetVoxelArray()[newIndex]._isUsed == 0)
                {
                    canSpreadY = false;
                }
            }
        }
        //If the box can spread, mark it as tested and increase the box size in the Y dimension.
        if (canSpreadY)
        {
            for (float x = boxStart.x; x < xLimit; x += this._voxelRender._voxelScale)
            {
                for (float z = boxStart.z; z < zLimit; z += this._voxelRender._voxelScale)
                {
                    float newY = boxStart.y + boxSize.y;
                    uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(x, newY, z);
 
                    tested[newIndex] = true;
                }
            }
            boxSize.y +=  this._voxelRender._voxelScale; ;
        }
        return canSpreadY;
    }

    //Returns whether the box can continue to spread along the positive Z axis.
    private bool TrySpreadZ(bool canSpreadZ, ref bool[] tested, Vector3 boxStart, ref Vector3 boxSize)
    {
        //Checks the square made by the X and Y size on the Z index one larger than the size of the
        //box.
        float xLimit = boxStart.x + boxSize.x;
        float yLimit = boxStart.y + boxSize.y;
        for (float x = boxStart.x; x < xLimit && canSpreadZ; x += this._voxelRender._voxelScale)
        {
            for (float y = boxStart.y; y < yLimit; y += this._voxelRender._voxelScale)
            {
                float newZ = boxStart.z + boxSize.z;
                uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(x, y, newZ);
                if (newZ >= this._voxelRender.Depth * this._voxelRender._voxelScale || tested[newIndex] || this._voxelRender.GetVoxelArray()[newIndex]._isUsed == 0)
                {
                    canSpreadZ = false;
                }
            }
        }
        //If the box can spread, mark it as tested and increase the box size in the Z dimension.
        if (canSpreadZ)
        {
            for (float x = boxStart.x; x < xLimit; x += this._voxelRender._voxelScale)
            {
                for (float y = boxStart.y; y < yLimit; y +=  this._voxelRender._voxelScale)
                {
                    float newZ = boxStart.z + boxSize.z;
                    uint newIndex = this._voxelRender.GetVoxelIndexFromPosition(x, y, newZ);
                    tested[newIndex] = true;
                }
            }
            boxSize.z += this._voxelRender._voxelScale; ;
        }
        return canSpreadZ;
    }

    //Applies the boxes passed to it to the collision mesh, reusing old boxes where it can.
    private void SetCollisionMesh(Dictionary<Vector3, Vector3> boxData)
    {
        int colliderIndex = 0;
        int existingColliderCount = m_colliders.Count;
        foreach (KeyValuePair<Vector3, Vector3> box in boxData)
        {
            //Position is the centre of the box collider for Unity3D.
            Vector3 position = (Vector3)box.Key + ((Vector3)box.Value / 2.0f);
            if (colliderIndex < existingColliderCount)  //If an old collider can be reused.
            {
                m_colliders[colliderIndex].center = position;
                m_colliders[colliderIndex].size = (Vector3)box.Value;
            }
            else  //Else if there were more boxes on this mesh generation than there were on the previous one.
            {
                GameObject boxObject = new GameObject(string.Format("Collider {0}", colliderIndex));
                BoxCollider boxCollider = boxObject.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                VoxelMeshCollider voxelMeshCollider = boxObject.AddComponent<VoxelMeshCollider>();
              
                voxelMeshCollider._voxelDestruction = this._voxelDestruction;
         //       voxelMeshCollider._playerVoxelColliderLayer = this._voxelDestruction._playerVoxelColliderLayer;

 
                Transform boxTransform = boxObject.transform;

                // Rigidbody rb = boxObject.AddComponent<Rigidbody>();
                //rb.useGravity = false;
                //    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //  rb.constraints = RigidbodyConstraints.FreezeAll;
                boxTransform.gameObject.layer = LayerMask.NameToLayer("Enemy");
                boxTransform.parent = transform;
                boxTransform.localPosition = Vector3.one * (-1) * this._voxelRender._voxelScale * 0.5f;
                boxTransform.localRotation = new Quaternion();
                boxCollider.center = position;
                boxCollider.size = (Vector3)box.Value;
                m_colliders.Add(boxCollider);
                boxTransform.localScale = Vector3.one;
            }
            ++colliderIndex;
        }
        //Deletes all the unused boxes if this mesh generation had less boxes than the previous one.
        if (colliderIndex < existingColliderCount)
        {
            for (int i = existingColliderCount - 1; i >= colliderIndex; --i)
            {
                Destroy(m_colliders[i].gameObject);
            }
            m_colliders.RemoveRange(colliderIndex, existingColliderCount - colliderIndex);
        }
    }
}