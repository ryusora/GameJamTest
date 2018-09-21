using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable/PlayerData")]
public class PlayerData : ScriptableObject {
	public Vector2 maxForce;
	public Vector2 dropForce;
    [Tooltip("Speed of the force ratio")]
    public float speed;
    [HideInInspector]
    public float forceRatio;

}
