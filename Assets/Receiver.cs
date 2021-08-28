using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public List<float> recievedSignals = new List<float>();

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<Wave>(out var wave);
        if (wave != null)
        {
            recievedSignals.Add(Time.fixedTime-wave.CreationTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.black;
        if(recievedSignals.Count!=0)
            Gizmos.DrawWireSphere(transform.position,recievedSignals.Last()*1000000/2);
    }
}
