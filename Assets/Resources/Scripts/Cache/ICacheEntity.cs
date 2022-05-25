using UnityEngine;

public interface ICacheEntity
{
    void Save();
    void Load();
    SaveObject GetSaveObject();
    GameObject GetGameObject();
}
