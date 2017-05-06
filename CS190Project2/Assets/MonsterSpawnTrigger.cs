using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTrigger : MonoBehaviour {

    public GameObject monster;

    public bool activated;

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.hasCode && other.CompareTag("Player") && !activated)
        {
            monster.SetActive(true);
            activated = true;
        }


    }
}
