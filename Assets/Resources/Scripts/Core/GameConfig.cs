using System;
using UnityEngine;

[Serializable]
public class GameConfig
{
    public bool shouldDisplayAds;
    public int afterGameAdChance;

    public static GameConfig CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameConfig>(jsonString);
    }
}