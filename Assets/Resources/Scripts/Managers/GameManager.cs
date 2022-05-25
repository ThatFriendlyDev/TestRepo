using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [HideInInspector]
    public EGameState state = EGameState.Waiting;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        this.SetState(EGameState.InProgress);
    }

    public void Play()
    {
        if (!this.IsWaiting())
        {
            return;
        }

        Profile.instance.gamesPlayed++;
        this.SetState(EGameState.InProgress);
    }

    public bool IsWaiting()
    {
        return this.state == EGameState.Waiting;
    }

    public bool IsInProgress()
    {
        return this.state == EGameState.InProgress;
    }

    public bool IsVictory()
    {
        return this.state == EGameState.Victory;
    }

    public bool IsFailed()
    {
        return this.state == EGameState.Failed;
    }

    public void SetState(EGameState state)
    {
        this.state = state;
    }
}
