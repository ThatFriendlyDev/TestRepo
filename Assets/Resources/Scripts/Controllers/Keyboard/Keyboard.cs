using UnityEngine;

public class Keyboard : MonoBehaviour, IInputController
{
    public Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }

        return direction;
    }

    EControllerType IInputController.Type
    {
        get
        {
            return EControllerType.Keyboard;
        }
    }

    bool IInputController.IsClickButtonPressed { get => Input.GetMouseButtonDown(0); }
}
