using TMPro;
using UnityEngine;

public class UIIncrementalAnnouncement : MonoBehaviour
{
    public RectTransform rect;
    public TextMeshProUGUI textMessage;

    [Header("Colors")]
    public Color colorPositive;
    public Color colorNegative;

    [Header("Config")]
    public float movementSpeed = 1.5f;

    [HideInInspector]
    public int amount;

    [HideInInspector]
    public string messageFormat;
    private Vector2 originPosition;

    private void Awake()
    {
        this.originPosition = this.rect.anchoredPosition;
    }

    private void Update()
    {
        this.textMessage.color = this.amount > 0 ? this.colorPositive : this.colorNegative;
        this.textMessage.SetText(this.GetMessage());
        this.rect.anchoredPosition += Vector2.up * this.movementSpeed;
    }

    public string GetMessage()
    {
        string amount = Utils.AbbreviateNumber(this.amount);

        if (string.IsNullOrEmpty(this.messageFormat))
        {
            return amount;
        }

        return this.messageFormat.Replace("{amount}", amount.ToString());
    }

    public void ResetOriginPosition()
    {
        this.rect.anchoredPosition = this.originPosition;
    }
}