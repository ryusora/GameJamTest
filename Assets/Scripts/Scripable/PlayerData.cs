using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerData : ScriptableObject {
#if UNITY_EDITOR
    [MenuItem("Menu/Scriptable/PlayerData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayerData>();
    }
#endif
	public float MAX_FORCE_LENGTH;
	public Vector2 startJumpForce;
	public Vector2 additionalJumpForce;
	public Vector2 hulkForce;
}
