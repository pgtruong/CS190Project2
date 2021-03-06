﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFootSounds : MonoBehaviour {

    bool isSprinting = false;

    void Start()
    {
        AkSoundEngine.SetState("Movement", "Walk");
        AkSoundEngine.PostEvent("Sprint", this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            AkSoundEngine.PostEvent("Footstep", this.gameObject);
    }

    public void Sprint(bool sprint)
    {
        if(sprint && !isSprinting)
        {
            isSprinting = true;
            AkSoundEngine.SetState("Movement", "Sprint");
        }
        if(!sprint && isSprinting)
        {
            isSprinting = false;
            AkSoundEngine.SetState("Movement", "Walk");
        }
    }
}
