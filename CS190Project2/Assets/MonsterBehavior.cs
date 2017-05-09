using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour {

    public bool spawned = false;
    public Transform target;
    public float speed = 1.0f;
    public bool seen = false;
    uint runEventID;

    public void Spawn()
    {

    }

    void Update()
    {
        if(!seen && spawned && this.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
        {
            AkSoundEngine.PostEvent("OPScream", target.gameObject);
            seen = true;
        }
    }

    void FixedUpdate()
    {
        if(spawned)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetDir = target.position - transform.position;
            targetDir.y = 0;
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            transform.forward = Vector3.Slerp(transform.forward, targetDir, Time.deltaTime * 10f);
            
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
        runEventID = AkSoundEngine.PostEvent("MonsterRun", this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            AkSoundEngine.PostEvent("MonsterFootstep", this.gameObject);
    }
}
