using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public bool Noise = true;

    Light flashLight;

    private void Start()
    {
        flashLight = GetComponent<Light>();
        StartCoroutine(LightNoise());
    }

    IEnumerator LightNoise()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 20f));
            flashLight.enabled = false;
            int count = Random.Range(1, 9);
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(Random.Range(0f, .3f));
                flashLight.enabled = !flashLight.enabled;
            }
            flashLight.enabled = true;
        }
    }
}
