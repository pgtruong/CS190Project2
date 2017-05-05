using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShotgun : MonoBehaviour
{

    public float rotateSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.FindChild("Main Camera").transform.FindChild("Shotgun").gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
