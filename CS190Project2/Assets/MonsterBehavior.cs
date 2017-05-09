﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour {

    public bool spawned = false;
    public Transform target;
    public float speed = 1.0f;
    public bool seen = false;
    uint runEventID;
    Animator animator;
    CapsuleCollider capCol;
    public int health = 2;

    void Start()
    {
        animator = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider>();
    }

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

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Roar"))
            capCol.enabled = false;
        else
            capCol.enabled = true;
        
    }

    void FixedUpdate()
    {
        if (spawned && animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
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

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                animator.SetTrigger("attack" + Random.Range(1, 3).ToString());
        }
    }
}
