using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityVector3Event : UnityEvent<Vector3> {}

public class Vector3EventListener : MonoBehaviour {
	[Tooltip("Event to register with.")]
    public Vector3Event Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityVector3Event Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector3 value)
    {
        Response.Invoke(value);
    }
}
