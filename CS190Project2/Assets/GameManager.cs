using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public bool hasCode = false;
    public bool unlockedDoor = false;
    public GameObject keypad;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    public void animateKeypad()
    {
        keypad.GetComponent<ParticleSystem>().Play();
    }
}
