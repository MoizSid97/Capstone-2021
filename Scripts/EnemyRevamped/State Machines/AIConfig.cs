using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIConfig : ScriptableObject
{
    public float maxTime = 1f;
    public float maxDistance = 1f;
    public float dieForce = 5.0f;
    public float maxSightDisance = 3.0f;
}
