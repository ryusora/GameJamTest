﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable/GameEvent")]
public class GameEvent : ScriptableObject {
	private readonly List<GameEventListener> eventListeners = 
		new List<GameEventListener>();

	public void Raise()
	{
		for(int i = eventListeners.Count -1; i >= 0; i--)
			eventListeners[i].OnEventRaised();
	}

	public void RegisterListener(GameEventListener listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(GameEventListener listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}
}
