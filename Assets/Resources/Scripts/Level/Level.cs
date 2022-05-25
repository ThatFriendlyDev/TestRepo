using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level singleton;
    public LevelEntityDatabase database;

    public MMSimpleObjectPooler playerBulletPooler;
    public MMSimpleObjectPooler enemyBulletPooler;

    [HideInInspector]
    public new Camera camera;

    [HideInInspector]
    public List<Multiplier> multipliers = new List<Multiplier>();

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }

        this.camera = Camera.main;
    }

    public void SwitchMultiplier(Multiplier target)
    {
        Utils.Shuffle(this.multipliers);

        foreach (Multiplier multiplier in this.multipliers)
        {
            if (multiplier == target)
            {
                continue;
            }

            int currentAmount = target.amount;
            target.amount = multiplier.amount;
            multiplier.amount = currentAmount;
            break;
        }
    }
}
