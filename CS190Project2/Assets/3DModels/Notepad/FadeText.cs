using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour {

    Text text;
    bool Edwin = false;
    bool corRun = false;

    private void Start()
    {
        text = GetComponent<Text>();
    }
    void Update () {        
		if (text.color.a != 0)
        {
            if (text.color.a == 1 && !Edwin)
                StartCoroutine(WaitSeconds(1));
            else if (Edwin && !corRun)
            {
                Color temp = text.color;
                temp.a -= Time.deltaTime;
                text.color = temp;
            }

        }
	}

    IEnumerator WaitSeconds(int seconds)
    {
        Edwin = true;
        corRun = Edwin;
        yield return new WaitForSeconds(seconds);
        corRun = false;
    }
}
