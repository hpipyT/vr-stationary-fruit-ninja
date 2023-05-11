using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerVoice : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    
    void Start()
    {




/*    public IEnumerator MoldStart()
    {
        // get list of all spawned objects
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            Debug.Log("stopping" + obj.name);

            // access their scripts

            // stop their movement
            Fruit objScript = obj.GetComponent<Fruit>();
            objScript.SetFruitSpeed(0);

        }
        yield return new WaitForSeconds(5.0f);
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {

            Debug.Log("starting" + obj.name);

            // access their scripts

            // start their movement

            if (obj.GetComponent<Fruit>())
            {
                Fruit objScript = obj.GetComponent<Fruit>();
                objScript.SetFruitSpeed(1.0f);
            }
        }


        yield return null;  */
    }

}
