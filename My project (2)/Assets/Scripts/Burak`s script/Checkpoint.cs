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
        //add code here to activate the checkpoint
        Debug.Log("Checkpoint Activated");
    }

    private void Deactivate()
    {
        //add code here to deactivate the checkpoint
        Debug.Log("Checkpoint Deactivated");
    }
}
