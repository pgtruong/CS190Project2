using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour {

    bool spawned = false;
    public Transform target;
    public float speed = 1.0f;

    public void Spawn()
    {

    }

    void FixedUpdate()
    {
        if(spawned)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetDir = target.position - transform.position;
            targetDir.y = 0;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            transform.forward = Vector3.Slerp(transform.forward, targetDir, Time.deltaTime * 2);
            
        }
    }

    void SpawnAnimStart()
    {
        AkSoundEngine.PostEvent("MonsterSpawn", this.gameObject);
    }

    void BeginRoar()
    {
        AkSoundEngine.PostEvent("MonsterRoar", this.gameObject);
    }

    void RoarCompleted()
    {
        spawned = true;
    }
}
