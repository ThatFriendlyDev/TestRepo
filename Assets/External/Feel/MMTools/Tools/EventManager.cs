using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static string ON_JOYSTICK_RELEASE = "ON_JOYSTICK_RELEASE_EVENT";


	public static string ON_ENEMY_SHOT = "ENEMY_SHOT_EVENT";
	public static string ON_DEATH_EVENT = "PLAYER_DEATH_EVENT";
	public static string ON_INIT_EVENT = "PLAYER_INIT_EVENT";

	private Dictionary<string, UnityEvent> _eventDictionary;

	private static EventManager eventManager;

	public static EventManager instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (!eventManager)
				{
					Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
				}
				else
				{
					eventManager.Init();
				}
			}

			return eventManager;
		}
	}

	private void Init()
	{
		if (this._eventDictionary == null)
		{
			this._eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent newEvent = null;

		if (instance._eventDictionary.TryGetValue(eventName, out newEvent))
		{
			newEvent.AddListener(listener);
		}
		else
		{
			newEvent = new UnityEvent();
			newEvent.AddListener(listener);
			instance._eventDictionary.Add(eventName, newEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if (eventManager == null)
		{
			return;
		}
		UnityEvent newEvent = null;

		if (instance._eventDictionary.TryGetValue(eventName, out newEvent))
		{
			newEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName)
	{
		UnityEvent theEvent = null;

		if (instance._eventDictionary.TryGetValue(eventName, out theEvent))
		{
			theEvent?.Invoke();
		}
	}
}
