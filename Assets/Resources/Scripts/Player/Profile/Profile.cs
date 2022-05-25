using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Profile
{
    public const string PlayerPrefsKey = "MY_Profile_";
    public const int PlayerPrefsVersionId = 1;

    public static Profile _instance;
    public static Profile instance
    {
        get
        {
            if (_instance == null)
            {
                if (PlayerPrefs.HasKey(GetPlayerPrefsKey()))
                {
                    _instance = Load();
                }
                else
                {
                    _instance = new Profile();
                }
            }

            return _instance;
        }
    }

    public int level = 1;
    public int dollars;
    public int gamesPlayed;

    public ESkinType activeSkin = ESkinType.Default;
    public ProfileSettings settings = new ProfileSettings();
    public LevelCache levelCache = new LevelCache();

    public List<ESkinType> skins = new List<ESkinType>()
    {
        ESkinType.Default
    };

    public string GetJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static Profile Load()
    {
        return JsonConvert.DeserializeObject<Profile>(PlayerPrefs.GetString(GetPlayerPrefsKey()));
    }

    public void Save()
    {
        PlayerPrefs.SetString(GetPlayerPrefsKey(), this.GetJson());
    }

    public void AddDollars(int amount)
    {
        this.dollars += amount;
    }

    public void RemoveDollars(int amount)
    {
        if (this.dollars - amount <= 0)
        {
            this.dollars = 0;
            return;
        }

        this.dollars -= amount;
    }

    public bool HasDollars(int amount)
    {
        return this.dollars >= amount;
    }

    public void IncrementLevel()
    {
        int levelCount = GlobalGameConfig.GetTotalLevelCount();

        if (this.level >= levelCount)
        {
            this.level = 1;
        }
        else
        {
            this.level += 1;
        }
    }

    private static string GetPlayerPrefsKey()
    {
        return PlayerPrefsKey + PlayerPrefsVersionId;
    }

    public void FlushLevelCache()
    {
        this.levelCache = new LevelCache();
    }
}