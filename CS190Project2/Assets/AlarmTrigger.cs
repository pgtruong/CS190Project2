using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour {

    public GameObject Alarms;
    public bool activated;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            foreach (StrobeLight strobeScript in Alarms.GetComponentsInChildren<StrobeLight>())
                strobeScript.enabled = true;
            activated = true;
        }


    }
}
