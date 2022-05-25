using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Components")]
    public Player player;

    [Header("Camera")]
    [SerializeField]
    public Vector3 cameraOffset;

    private void LateUpdate()
    {
        //Level.singleton.camera.transform.position = this.transform.position + this.cameraOffset;
    }
}
