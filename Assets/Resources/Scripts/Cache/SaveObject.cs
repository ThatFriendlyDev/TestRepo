using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class SaveObject
{
    public string key;
    public List<SaveEntity> values = new List<SaveEntity>();

    public void Add<T>(string key, T value)
    {
        SaveEntity entity = this.GetSaveEntity(key);

        if (entity != null)
        {
            entity.key = key;
            entity.value = JsonConvert.SerializeObject(value);
            return;
        }

        this.values.Add(new SaveEntity
        {
            key = key,
            value = JsonConvert.SerializeObject(value)
        });
    }

    public T Get<T>(string key)
    {
        foreach (SaveEntity entity in this.values)
        {
            if (entity.key == key)
            {
                return JsonConvert.DeserializeObject<T>(entity.value);
            }
        }

        return default;
    }

    public void Remove(string key)
    {
        SaveEntity entity = this.GetSaveEntity(key);

        if (entity == null)
        {
            return;
        }

        this.values.Remove(entity);
    }

    private SaveEntity GetSaveEntity(string key)
    {
        foreach (SaveEntity entity in this.values)
        {
            if (entity.key == key)
            {
                return entity;
            }
        }

        return null;
    }

    public void Flush()
    {
        this.values.Clear();
    }
}
