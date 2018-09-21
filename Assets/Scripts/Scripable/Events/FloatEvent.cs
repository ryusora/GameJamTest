using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "Scriptable/FloatEvent")]
public class FloatEvent : ScriptableObject {
	private readonly List<FloatEventListener> eventListeners = 
		new List<FloatEventListener>();

	public void Raise(float value)
	{
		for(int i = eventListeners.Count -1; i >= 0; i--)
			eventListeners[i].OnEventRaised(value);
	}

	public void RegisterListener(FloatEventListener listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(FloatEventListener listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}
}