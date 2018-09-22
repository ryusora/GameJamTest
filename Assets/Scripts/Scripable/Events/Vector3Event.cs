using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Event", menuName = "Scriptable/Vector3Event")]
public class Vector3Event : ScriptableObject {
	private readonly List<Vector3EventListener> eventListeners = 
		new List<Vector3EventListener>();

	public void Raise(Vector3 value)
	{
		for(int i = eventListeners.Count -1; i >= 0; i--)
			eventListeners[i].OnEventRaised(value);
	}

	public void RegisterListener(Vector3EventListener listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(Vector3EventListener listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}
}