using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player singleton;

    [Header("Components")]
    public PlayerController controller;
    public new PlayerAnimation animation;
    public Animator weaponAnimator;
    public PlayerLook look;
    public TextMeshPro textBulletCount;

    public ProjectileWeapon projectileWeapon;
    public MMSimpleObjectPooler objectPooler;
    public Transform weaponSpawnLocation;
    public int startingBulletCount = 30;

    [HideInInspector]
    public EPlayerState state;

    [HideInInspector]
    public int currentBulletCount;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        this.currentBulletCount = this.startingBulletCount;
        this.InvokeRepeating(nameof(this.Shoot), 0.25f, 0.25f);
    }

    private void Update()
    {
        this.textBulletCount.text = this.currentBulletCount.ToString();    
    }

    private void Shoot()
    {
        if (this.currentBulletCount <= 0)
        {
            return;
        }

        this.weaponAnimator.SetTrigger("Shoot");
        projectileWeapon.ShootWithWeapon();
        this.currentBulletCount--;
    }
}
