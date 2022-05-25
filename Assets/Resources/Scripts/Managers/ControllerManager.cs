using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static IInputController input;
    public GameObject[] controllers;
    public Transform parent;

    private void Awake()
    {
        EControllerType controllerType = EControllerType.Keyboard;

        if (BuildManager.IsPhone())
        {
            controllerType = EControllerType.Joystick;
        }

        GameObject go = Instantiate(this.GetByType(controllerType));
        go.transform.SetParent(this.parent, false);
        input = go.GetComponent<IInputController>();
    }

    private GameObject GetByType(EControllerType type)
    {
        foreach (GameObject go in this.controllers)
        {
            IInputController controller = go.GetComponent<IInputController>();

            if (controller.Type == type)
            {
                return go;
            }
        }

        return null;
    }
}
