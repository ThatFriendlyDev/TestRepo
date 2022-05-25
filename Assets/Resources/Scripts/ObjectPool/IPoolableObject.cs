using UnityEngine;

public interface IPoolableObject
{
    public void PrepareForPool();
    public GameObject GetGameObject();
    public EPoolableObjectType GetType();
}
