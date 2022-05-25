using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource effectsAudioSource;

    [Header("Sounds")]
    public AudioClip buttonSound;
    public AudioClip moneyPickupSound;
    public AudioClip[] footsteps;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        this.MuteEverything(!Profile.instance.settings.isMusicEnabled);
    }

    public void PlayEffect(AudioClip clip)
    {
        if (!this.effectsAudioSource.enabled)
        {
            return;
        }

        this.effectsAudioSource.PlayOneShot(clip);
    }

    public void MuteMusic(bool value)
    {
        this.musicAudioSource.enabled = !value;
    }

    public void MuteEffects(bool value)
    {
        this.effectsAudioSource.enabled = !value;
    }

    public void MuteEverything(bool value)
    {
        this.MuteMusic(value);
        this.MuteEffects(value);
    }

    public AudioClip GetRandomFootstep()
    {
        return this.footsteps[Random.Range(0, this.footsteps.Length)];
    }
}
