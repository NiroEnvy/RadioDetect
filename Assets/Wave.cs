using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Wave : MonoBehaviour
{
    public float CreationTime { get; private set; }
    private float ScaleSpeed { get; set; }
    
    private void Start()
    {
        CreationTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        transform.localScale+=Vector3.one * (ScaleSpeed *Time.fixedDeltaTime);
    }

    public void SetSpeed(float speed)
    {
        if (speed >= 0) ScaleSpeed = speed;
        else throw new ArgumentException("Negative speed of wave");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        var transform1 = transform;
        Gizmos.DrawWireSphere(transform1.position,1f*transform1.localScale.x);
    }
}
