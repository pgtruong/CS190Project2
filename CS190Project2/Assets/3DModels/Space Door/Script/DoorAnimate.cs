using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour {
    GameObject door;

    void Start()
    {
        door = GameObject.FindGameObjectWithTag("SF_Door");    
    }

    void OnTriggerEnter(Collider other)
    {
        door.GetComponent<Animation>().Play("open");
    }

    void OnTriggerExit(Collider other)
    {
        door.GetComponent<Animation>().Play("close");    
    }
}
