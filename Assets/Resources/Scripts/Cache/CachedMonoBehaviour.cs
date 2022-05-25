using UnityEngine;

public abstract class CachedMonoBehaviour : MonoBehaviour, ICacheEntity
{
    private string uniqueId;

    public virtual void Start()
    {
        this.uniqueId = Level.singleton.database.GetId(this.gameObject);
        this.Load();
    }

    public virtual void Update()
    {
        this.Save();
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public SaveObject GetSaveObject()
    {
        return Profile.instance.levelCache.GetOrSet(this.uniqueId);
    }

    public abstract void Save();
    public abstract void Load();
}
