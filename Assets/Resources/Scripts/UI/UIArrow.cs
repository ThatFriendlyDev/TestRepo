using UnityEngine;
using UnityEngine.UI;

public class UIArrow : MonoBehaviour
{
    public static UIArrow singleton;

    public Image image;
    public float screenBoundOffset = 0.9f;

    private Vector3 screenCenter;
    private Vector3 screenBounds;

    [HideInInspector]
    public Vector3 target;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }

        this.screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        this.screenBounds = this.screenCenter * this.screenBoundOffset;
    }

    private void Update()
    {
        if (!GameManager.singleton.IsInProgress())
        {
            this.image.enabled = false;
            return;
        }

        if (this.target == Vector3.zero)
        {
            this.image.enabled = false;
            return;
        }

        Vector3 screenPosition = Utils.GetScreenPosition(Level.singleton.camera, this.target);
        bool isTargetVisible = Utils.IsTargetVisible(screenPosition);

        if (isTargetVisible)
        {
            this.image.enabled = false;
            return;
        }

        float angle = float.MinValue;
        Utils.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, this.screenCenter, this.screenBounds);
        this.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        this.transform.position = screenPosition;
        this.image.enabled = true;
    }

    public void RemoveTarget()
    {
        this.target = Vector3.zero;
    }
}
