using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public bool hasCode = false;
    public bool unlockedDoor = false;
    public GameObject keypad;

    void Awake()
    {

    }

    void Start()
    {
        hasCode = false;
        unlockedDoor = false;
        AkSoundEngine.SetRTPCValue("PlayerHealth", 3);
    }

    public void animateKeypad()
    {
        keypad.GetComponent<ParticleSystem>().Play();
    }
}
