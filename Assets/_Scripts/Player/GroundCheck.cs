using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    public UnityEvent<bool> GroundCollided = new UnityEvent<bool>();

    private void OnTriggerEnter(Collider other)
    {
        GroundCollided.Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        GroundCollided.Invoke(false);
    }
}
