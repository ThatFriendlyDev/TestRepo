using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    public new Renderer renderer;
    public int amount = 2;
    public TextMeshPro textAmount;
    public int maxHitCount = 5;

    [Header("Colors")]
    public Color colorLow;
    public Color colorMedium;
    public Color colorAverage;
    public Color colorHigh;

    [SerializeField]
    private Vector3 offsetToInitialPosition;

    [SerializeField]
    private MMFeedbacks onHitByBulletFeedback;
    private int currentHitCount;

	private void Start()
	{
        Level.singleton.multipliers.Add(this);
        transform.DOLocalMove(transform.localPosition + offsetToInitialPosition, Random.Range(2, 3)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	private void Update()
    {
        this.textAmount.text = this.amount.ToString() + "X";
        this.renderer.material.color = this.GetColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ETag.Bullet.ToString()))
        {
            return;
        }

        Bullet bulletToSpawnFrom = other.GetComponent<Bullet>();

        if (!bulletToSpawnFrom.canMultiply)
        {
            return;
        }

        this.currentHitCount++;

        if (this.currentHitCount >= this.maxHitCount)
        {
            Level.singleton.SwitchMultiplier(this);
            this.currentHitCount = 0;
        }

        bulletToSpawnFrom.canMultiply = false;

        List<Rigidbody> bulletsToSpread = new List<Rigidbody>();
        bulletsToSpread.Add(bulletToSpawnFrom.GetComponent<Rigidbody>());

        for (int i = 0; i < this.amount - 1; i++)
        {
            GameObject go;

            if (bulletToSpawnFrom.projectile.GetOwner() == Player.singleton.gameObject)
            {
                go = Level.singleton.playerBulletPooler.GetPooledGameObject();
            }
            else
            {
                go = Level.singleton.enemyBulletPooler.GetPooledGameObject();
            }

            // Spawning a new bullet
            {
                Bullet spawnedBullet = go.GetComponent<Bullet>();
                bulletsToSpread.Add(spawnedBullet.GetComponent<Rigidbody>());
                spawnedBullet.canMultiply = false;
                spawnedBullet.Owner = bulletToSpawnFrom.Owner;
                go.transform.position = Utils.GetPositionAroundObject(other.transform.position, 0.2f);
                go.SetActive(true);
            }
        }

        bulletsToSpread.ForEach(bulletRb => {

            bulletRb.isKinematic = false;
            bulletRb.useGravity = true;
            bulletRb.AddForce(new Vector3(Random.Range(-500, 500), 0, 0));
            });

        onHitByBulletFeedback.PlayFeedbacks();
    }


	private void OnTriggerExit(Collider other)
	{
        if (!other.CompareTag(ETag.Bullet.ToString()))
        {
            return;
        }

        Bullet bulletExited = other.GetComponent<Bullet>();
        bulletExited.canMultiply = true;
    }

    private Color GetColor()
    {
        if (this.amount > 2 && this.amount <= 4)
        {
            return this.colorMedium;
        }

        if (this.amount > 4 && this.amount <= 6)
        {
            return this.colorAverage;
        }

        if (this.amount > 6 && this.amount <= 8)
        {
            return this.colorHigh;
        }

        return this.colorLow;
    }
}
