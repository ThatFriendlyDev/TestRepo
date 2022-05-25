using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntityDatabase : MonoBehaviour
{
    [HideInInspector]
    public List<LevelEntityId> entities = new List<LevelEntityId>();

    public bool Has(GameObject go)
    {
        foreach (LevelEntityId entity in this.entities)
        {
            if (entity.go == go)
            {
                return true;
            }
        }

        return false;
    }

    public string GetId(GameObject go)
    {
        foreach (LevelEntityId entity in this.entities)
        {
            if (entity.go == go)
            {
                return entity.uniqueId;
            }
        }

        throw new Exception("Missing cache ID!");
    }
}
