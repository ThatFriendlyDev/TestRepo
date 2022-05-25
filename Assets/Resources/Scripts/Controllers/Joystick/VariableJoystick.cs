using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick, IInputController
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    private Vector2 fixedPosition = Vector2.zero;

    protected override void Start()
    {
        base.Start();
        this.fixedPosition = this.background.anchoredPosition;
        this.background.anchoredPosition = this.fixedPosition;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        this.background.anchoredPosition = this.ScreenPointToAnchoredPosition(eventData.position);
        this.background.gameObject.SetActive(Profile.instance.settings.isJoystickEnabled);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        this.background.anchoredPosition = this.fixedPosition;
        this.background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);
    }

    bool IInputController.IsClickButtonPressed
    {
        get
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }

    EControllerType IInputController.Type
    {
        get
        {
            return EControllerType.Joystick;
        }
    }
}