using System;
using System.Collections.Generic;

[Serializable]
public class LevelCache
{
    public int id;
    public List<SaveObject> saveObjects = new List<SaveObject>();

    public SaveObject Get(string key)
    {
        foreach (SaveObject saveObject in this.saveObjects)
        {
            if (saveObject.key == key)
            {
                return saveObject;
            }
        }

        return null;
    }

    public SaveObject GetOrSet(string key)
    {
        SaveObject ob = this.Get(key);

        if (ob != null)
        {
            return ob;
        }

        ob = new SaveObject
        {
            key = key
        };

        this.saveObjects.Add(ob);

        return ob;
    }

    public void Remove(string key)
    {
        SaveObject ob = this.Get(key);

        if (ob == null)
        {
            return;
        }

        this.saveObjects.Remove(ob);
    }
}