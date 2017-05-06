using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadLogic : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.hasCode && !GameManager.instance.unlockedDoor)
        {
            AkSoundEngine.PostEvent("UnlockDoor", this.gameObject);
            GameManager.instance.unlockedDoor = true;
            this.GetComponent<ParticleSystem>().Stop();
        }
    }
}
