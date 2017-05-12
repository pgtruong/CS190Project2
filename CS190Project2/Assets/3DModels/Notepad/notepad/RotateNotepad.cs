using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateNotepad : MonoBehaviour {

    public float speed = 10f;
    public GameManager gm;
    public Text pickup;

    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Color temp = pickup.color;
            temp.a = 1;
            pickup.color = temp;
            AkSoundEngine.PostEvent("Pickup_Code", gameObject);
            Destroy(gameObject);
            gm.hasCode = true;
            gm.animateKeypad();
        }
    }
}
