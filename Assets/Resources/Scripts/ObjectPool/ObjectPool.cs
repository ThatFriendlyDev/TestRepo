using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool singleton;

    [Header("Prefabs")]
    public PoolableObject[] poolableObjects;

    private List<IPoolableObject> objects = new List<IPoolableObject>();

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }

        foreach (PoolableObject poolableObject in this.poolableObjects)
        {
            for (int i = 0; i < poolableObject.count; i++)
            {
                IPoolableObject po = this.Create(poolableObject.prefab);
                this.objects.Add(po);
            }
        }
    }

    public void Add(IPoolableObject po)
    {
        if (po.GetGameObject().transform.parent != this.transform)
        {
            po.GetGameObject().transform.SetParent(this.transform);
        }

        po.PrepareForPool();
        po.GetGameObject().SetActive(false);
    }

    public GameObject Get(EPoolableObjectType type)
    {
        IPoolableObject po = this.GetOrCreate(type);
        po.GetGameObject().SetActive(true);

        return po.GetGameObject();
    }

    private IPoolableObject Create(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(this.transform);
        go.SetActive(false);

        return go.GetComponent<IPoolableObject>();
    }

    private IPoolableObject GetOrCreate(EPoolableObjectType type)
    {
        foreach (IPoolableObject po in this.objects)
        {
            if (po.GetType() != type)
            {
                continue;
            }

            if (po.GetGameObject().activeSelf)
            {
                continue;
            }

            return po;
        }

        foreach (PoolableObject poolableObject in this.poolableObjects)
        {
            IPoolableObject po = poolableObject.prefab.GetComponent<IPoolableObject>();

            if (po.GetType() == type)
            {
                return this.Create(poolableObject.prefab);
            }
        }

        return null;
    }
}
