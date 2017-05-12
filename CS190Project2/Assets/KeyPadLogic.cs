using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadLogic : MonoBehaviour {

    public GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gm.hasCode && !gm.unlockedDoor)
        {
            AkSoundEngine.PostEvent("UnlockDoor", this.gameObject);
            gm.unlockedDoor = true;
            this.GetComponent<ParticleSystem>().Stop();
        }
    }
}
