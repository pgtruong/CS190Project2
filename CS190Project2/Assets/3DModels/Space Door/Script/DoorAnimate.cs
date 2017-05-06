using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour {
    GameObject door;
    public bool opened = false;

    void Start()
    {
        door = GameObject.FindGameObjectWithTag("SF_Door");    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.unlockedDoor && !opened)
        {
            door.GetComponent<Animation>().Play("open");
            AkSoundEngine.PostEvent("DoorOpen", this.gameObject);
            opened = true;
        }
    }
}
