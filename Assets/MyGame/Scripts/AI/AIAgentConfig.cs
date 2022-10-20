using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float minDistance = 1.0f;
    public float dieForce = 10f;
    public float maxSightDistance = 5.0f;
}
