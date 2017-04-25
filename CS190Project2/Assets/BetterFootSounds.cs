using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFootSounds : MonoBehaviour {

    int counter = 0;

    void Start()
    {
        AkSoundEngine.SetState("Movement", "Walk");
        AkSoundEngine.PostEvent("Sprint", this.gameObject);
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        counter++;
        AkSoundEngine.PostEvent("Footstep", this.gameObject);
    }
}
