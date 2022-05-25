using UnityEngine;

public class UIIncrementalAnnouncements : MonoBehaviour
{
    public static UIIncrementalAnnouncements singleton;

    public GameObject panel;
    public UIIncrementalAnnouncement slot;
    public Transform content;
    public float duration = 1.5f;

    private UIIncrementalAnnouncement currentAnnouncement;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    public void Show(int amount, string messageFormat = "", Vector3 position = default)
    {
        if (this.currentAnnouncement)
        {
            this.currentAnnouncement.amount += amount;
            this.currentAnnouncement.ResetOriginPosition();
            this.CancelInvoke();
            this.Invoke(nameof(this.DestroyCurrentAnnouncement), this.duration);
            return;
        }

        GameObject go = Instantiate(this.slot.gameObject);
        go.transform.SetParent(this.content, false);

        UIIncrementalAnnouncement slot = go.GetComponent<UIIncrementalAnnouncement>();
        slot.amount = amount;
        slot.messageFormat = messageFormat;

        if (position != Vector3.zero)
        {
            slot.rect.position = Utils.GetScreenPosition(Level.singleton.camera, position);
        }

        this.currentAnnouncement = slot;
        this.Invoke(nameof(this.DestroyCurrentAnnouncement), this.duration);
    }

    private void DestroyCurrentAnnouncement()
    {
        if (!this.currentAnnouncement)
        {
            return;
        }

        Destroy(this.currentAnnouncement.gameObject);
        this.currentAnnouncement = null;
    }
}
