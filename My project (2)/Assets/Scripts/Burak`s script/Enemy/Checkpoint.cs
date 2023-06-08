using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint CurrentCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CurrentCheckpoint != null)
            {
                CurrentCheckpoint.Deactivate();
            }

            CurrentCheckpoint = this;
            Activate();
        }
    }

    private void Activate()
    {
        
    }

    private void Deactivate()
    {
        
    }
}
