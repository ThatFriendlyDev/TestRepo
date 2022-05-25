using DG.Tweening;
using UnityEngine;

public class PowerUpCotroller : MonoBehaviour
{
    [SerializeField]
    private float maxScalingCoefficient;

    private void Start()
    {
        transform.DOScale(maxScalingCoefficient * Vector3.one, Random.Range(1, 1.5f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void FixedUpdate()
    {
        //this.transform.Rotate(new Vector3(0f, Time.fixedDeltaTime * 20, 0f), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ETag.Player.ToString()))
        {
            return;
        }

        Player.singleton.currentBulletCount += 15;
        this.gameObject.SetActive(false);
    }
}
