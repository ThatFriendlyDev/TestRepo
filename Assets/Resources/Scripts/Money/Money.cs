using MoreMountains.NiceVibrations;
using UnityEngine;

public class Money : DestinationActionEntity
{
    public float rotationDegreesPerSecond = 40f;

    [HideInInspector]
    public int value;
    private Vector3 originalScale;

    private void Awake()
    {
        this.originalScale = this.transform.localScale;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.transform.Rotate(new Vector3(0f, Time.fixedDeltaTime * this.rotationDegreesPerSecond, 0f), Space.World);
    }

    public void PrepareForPool()
    {
        this.value = 0;
        this.destination = null;
        this.collider.enabled = true;
        this.rigidbody.isKinematic = true;
        this.transform.localScale = this.originalScale;
    }

    public void OnPickup()
    {
        string messageFormat;

        if (this.value > 0)
        {
            messageFormat = "+${amount}";
        }
        else
        {
            messageFormat = "-${amount}";
        }

        Profile.instance.AddDollars(this.value);
        UIIncrementalAnnouncements.singleton.Show(this.value, messageFormat);
        UICanvas.singleton.SpawnMoney(this.value, this.transform.position);

        if (Profile.instance.settings.useVibrations)
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        SoundManager.singleton.PlayEffect(SoundManager.singleton.moneyPickupSound);
        Destroy(this.gameObject);
    }
}
