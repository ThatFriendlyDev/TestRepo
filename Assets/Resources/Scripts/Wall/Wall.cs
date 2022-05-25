using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int targetAmount;
    public TextMeshPro textAmount;
    public int fractureOnAmountLost = 100;
    private int fractureOnAmountLostCounter;
    [SerializeField]
    private MMFeedbacks onBulletHitFeedback;

    [HideInInspector]
    public int currentAmount;

    [SerializeField]
    private FractureCtrl fractureCtrl;

    private void Start()
    {
        this.currentAmount = this.targetAmount;
        fractureOnAmountLostCounter = fractureOnAmountLost;
    }

    private void Update()
    {
        this.textAmount.text = this.currentAmount.ToString();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag(ETag.Bullet.ToString()))
        {
            return;
        }

        if (this.currentAmount < 0)
		{
            textAmount.gameObject.SetActive(false);
            return;
		}
             
        this.currentAmount--;
        fractureOnAmountLostCounter--;
        if (fractureOnAmountLostCounter < 0)
		{
            fractureOnAmountLostCounter = fractureOnAmountLost;
            fractureCtrl.Fracture();
        }

        if (currentAmount < 0)
		{
            fractureCtrl.FractureExplode();
		}

        collider.gameObject.SetActive(false);
        onBulletHitFeedback.PlayFeedbacks();
    }


}
