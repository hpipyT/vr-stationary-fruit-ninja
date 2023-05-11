using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource source;
    public Vector3 minscale;
    public Vector3 maxscale;
    public AudioLoudnessDetector detector;

    bool moldingStarted = false;

    void Start()
    {
        foreach (string mic in Microphone.devices)
        {
            Debug.Log(mic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone();

        if (loudness > 0.3f)
        {
            moldingStarted = true;
            StartCoroutine(MoldStart());
        }

        // transform.localScale = Vector3.Lerp(minscale, maxscale, loudness);
    }

    public IEnumerator MoldStart()
    {
        // get list of all spawned objects
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            

            // access their scripts

            // stop their movement

            if (obj.GetComponent<Fruit>())
            {
                //Debug.Log("stopping " + obj.name);
                obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Fruit objScript = obj.GetComponent<Fruit>();
                objScript.moveEnabled = false;
            }

        }
        yield return new WaitForSeconds(7.50f);
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {

            

            // access their scripts

            // start their movement

            if (obj.GetComponent<Fruit>())
            {
                //Debug.Log("starting " + obj.name);
                Fruit objScript = obj.GetComponent<Fruit>();
                objScript.moveEnabled = true;

            }
        }


        yield return null;
    }


}
