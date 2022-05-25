using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public static UICanvas singleton;

    [Header("Money")]
    public Sprite moneyIcon;
    public Transform moneyParent;
    public RectTransform moneyDestination;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
    }

    public void Update()
    {
        this.CheckForUIButtonClick();
    }

    public void SpawnMoney(int amount, Vector3 position)
    {
        for (int i = 0; i < Mathf.Clamp(amount, 0, 100); i++)
        {
            Vector3 target = Utils.GetPositionAroundObject(position, 1);

            GameObject go = ObjectPool.singleton.Get(EPoolableObjectType.InterfaceDestinationActionEntity);
            go.transform.SetParent(this.moneyParent, false);
            go.transform.position = Utils.GetScreenPosition(Level.singleton.camera, target);

            UIDestinationActionEntity entity = go.GetComponent<UIDestinationActionEntity>();
            entity.image.sprite = this.moneyIcon;
            entity.destination = Utils.SwitchToRectTransform(this.moneyDestination, entity.rect);
        }
    }

    private void CheckForUIButtonClick()
    {
        if (!ControllerManager.input.IsClickButtonPressed)
        {
            return;
        }

        bool isPointerOverGameObject;

        if (BuildManager.IsPhone())
        {
            isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        else
        {
            isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
        }

        if (isPointerOverGameObject)
        {
            GameObject go = EventSystem.current.currentSelectedGameObject;

            if (go && go.GetComponent<Button>())
            {
                SoundManager.singleton.PlayEffect(SoundManager.singleton.buttonSound);
                return;
            }
        }

        GameManager.singleton.Play();
    }
}
