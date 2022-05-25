using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDestinationActionEntity : MonoBehaviour, IPoolableObject
{
    public RectTransform rect;
    public Image image;
    public float movementSpeed = 15f;

    [HideInInspector]
    public Vector2 destination;

    [HideInInspector]
    public Action callback;

    private void Update()
    {
        if (Vector2.Distance(this.rect.anchoredPosition, this.destination) <= 0)
        {
            if (this.callback != null)
            {
                this.callback.Invoke();
            }

            ObjectPool.singleton.Add(this);
            return;
        }

        this.rect.anchoredPosition = Vector2.MoveTowards(this.rect.anchoredPosition, this.destination, this.movementSpeed);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void PrepareForPool()
    {
        this.callback = null;
        this.destination = Vector2.zero;
    }

    EPoolableObjectType IPoolableObject.GetType()
    {
        return EPoolableObjectType.InterfaceDestinationActionEntity;
    }
}
