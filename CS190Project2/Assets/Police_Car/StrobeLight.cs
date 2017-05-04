using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrobeLight : MonoBehaviour {

    public float time = 0.5f;

    void Start()
    {
        {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            GetComponent<Light>().enabled = false;
            yield return new WaitForSeconds(time);
            GetComponent<Light>().enabled = true;
            AkSoundEngine.PostEvent("AlarmFlare", this.gameObject);
            yield return new WaitForSeconds(time);
        }
    }

}
