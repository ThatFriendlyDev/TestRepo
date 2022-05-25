using UnityEngine;

public class UIShop : MonoBehaviour
{
    public static UIShop singleton;
    public GameObject panel;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    public void Open()
    {
        this.panel.SetActive(true);
    }

    public void Close()
    {
        this.panel.SetActive(false);
    }
}
