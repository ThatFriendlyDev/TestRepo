using UnityEngine;
using TMPro;

public class UIGame : MonoBehaviour
{
    public static UIGame instance;
    public GameObject tutorial;

    [Header("Config")]
    public bool isShopEnabled;

    [Header("Dollars")]
    public GameObject dollars;
    public TextMeshProUGUI textDollars;

    [Header("Shop")]
    public GameObject shopButton;

    public void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        this.shopButton.SetActive(this.isShopEnabled);
    }

    private void Update()
    {
        this.dollars.SetActive(this.ShouldShowDollars());
        this.textDollars.text = Utils.AbbreviateNumber(Profile.instance.dollars).ToString();
        this.tutorial.SetActive(GameManager.singleton.state == EGameState.Waiting);
    }

    private bool ShouldShowDollars()
    {
        if (GameManager.singleton.IsVictory())
        {
            return false;
        }

        if (GameManager.singleton.IsFailed())
        {
            return false;
        }

        return true;
    }
}