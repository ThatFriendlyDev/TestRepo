using UnityEngine;

public interface IInputController
{
    Vector3 GetDirection();
    EControllerType Type { get; }
    bool IsClickButtonPressed { get; }
}