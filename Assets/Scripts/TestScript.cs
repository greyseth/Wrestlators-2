using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    float clickAmount, t, maxDelay = .5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(Testing());
        }
    }

    IEnumerator Testing()
    {
        GetComponent<PlayerMovement>().enabled = false;
        yield return null;
    }

    bool DoubleClick(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            clickAmount++;
            if (clickAmount == 1) t = Time.time;
        }
        if (clickAmount > 1 && Time.time - t < maxDelay)
        {
            clickAmount = 0;
            t = 0;
            return true;
        }
        else if (clickAmount > 2 || Time.time - t > 1) clickAmount = 0;
        return false;
    }
}
