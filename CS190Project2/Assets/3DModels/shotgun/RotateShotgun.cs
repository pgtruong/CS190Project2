using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateShotgun : MonoBehaviour
{

    public float rotateSpeed = 10f;
    public Text pickup;
    public Image reticle;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.FindChild("Main Camera").transform.FindChild("Shotgun").gameObject.SetActive(true);
            AkSoundEngine.PostEvent("Pickup_Shotgun", this.gameObject);
            Color temp = pickup.color;
            temp.a = 1;
            pickup.color = temp;
            reticle.enabled = true;
            Destroy(gameObject);
        }
    }
}
