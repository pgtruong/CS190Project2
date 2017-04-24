using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFootSounds : MonoBehaviour {

    int counter = 0;

    void OnTriggerEnter(Collider other)
    {
        counter++;
        AkSoundEngine.PostEvent("Footstep", this.gameObject);
    }
}
