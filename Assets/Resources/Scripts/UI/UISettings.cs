using TMPro;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public static UISettings singleton;
    public GameObject panel;

    [Header("Buttons")]
    public GameObject restorePurchases;
    public GameObject privacyPolicy;

    [Header("Vibrations")]
    public GameObject vibrationsOn;
    public GameObject vibrationsOff;

    [Header("Sounds")]
    public GameObject soundOn;
    public GameObject soundOff;

    [Header("Joystick")]
    public GameObject joystickOn;
    public GameObject joystickOff;

    [Header("Texts")]
    public TextMeshProUGUI textVersion;

    [Header("Config")]
    public bool isRestorePurchasesEnabled;
    public bool isPrivacyPolicyEnabled;
    public int requiredDebugButtonClickCount = 4;

    private int debugButtonClickedCount;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }

        this.textVersion.text = "v" + Application.version;
        this.panel.SetActive(false);

        this.restorePurchases.SetActive(this.ShouldEnableRestorePurchasesButton());
        this.privacyPolicy.SetActive(this.isPrivacyPolicyEnabled);
    }

    private void Start()
    {
        this.debugButtonClickedCount = 0;    
    }

    private void Update()
    {
        if (!this.panel.activeSelf)
        {
            return;
        }

        this.vibrationsOn.SetActive(Profile.instance.settings.useVibrations);
        this.vibrationsOff.SetActive(!Profile.instance.settings.useVibrations);

        this.soundOn.SetActive(Profile.instance.settings.isMusicEnabled);
        this.soundOff.SetActive(!Profile.instance.settings.isMusicEnabled);

        this.joystickOn.SetActive(Profile.instance.settings.isJoystickEnabled);
        this.joystickOff.SetActive(!Profile.instance.settings.isJoystickEnabled);
    }

    public void Open()
    {
        this.panel.SetActive(true);
    }

    public void Close()
    {
        this.panel.SetActive(false);
    }

    public void OnMusicSliderValueChanged()
    {
        bool value = Profile.instance.settings.isMusicEnabled;

        Profile.instance.settings.isMusicEnabled = !value;
        SoundManager.singleton.MuteEverything(value);
    }

    public void OnVibrationsSliderValueChanged()
    {
        bool value = Profile.instance.settings.useVibrations;
        Profile.instance.settings.useVibrations = !value;
    }

    public void OnJoystickSliderValueChanged()
    {
        bool value = Profile.instance.settings.isJoystickEnabled;
        Profile.instance.settings.isJoystickEnabled = !value;
    }

    public void OnRestorePurchasesButtonClicked()
    {

    }

    public void OnPrivacyPolicyButtonClicked()
    {

    }

    private bool ShouldEnableRestorePurchasesButton()
    {
        if (!this.isRestorePurchasesEnabled)
        {
            return false;
        }

#if UNITY_ANDROID
    return false;
#endif

        return true;
    }

    public void OnDebugButtonClicked()
    {
        bool value = Profile.instance.settings.isDebugModeEnabled;

        if (this.debugButtonClickedCount >= this.requiredDebugButtonClickCount)
        {
            Profile.instance.settings.isDebugModeEnabled = !value;
            this.debugButtonClickedCount = 0;
            return;
        }

        this.debugButtonClickedCount++;
    }
}
