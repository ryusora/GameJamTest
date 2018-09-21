using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> {
	
}

public class FloatEventListener : MonoBehaviour {
	[Tooltip("Event to register with.")]
    public FloatEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityFloatEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(float value)
    {
        Response.Invoke(value);
    }
}
