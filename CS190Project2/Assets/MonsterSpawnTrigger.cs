using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTrigger : MonoBehaviour {

    public GameObject monster;

    public bool activated;

    public GameManager gm;

    void OnTriggerEnter(Collider other)
    {
        if (gm.hasCode && other.CompareTag("Player") && !activated)
        {
            monster.SetActive(true);
            activated = true;
        }


    }
}
