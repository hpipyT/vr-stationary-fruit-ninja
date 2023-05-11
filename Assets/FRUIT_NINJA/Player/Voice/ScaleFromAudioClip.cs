using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource source;
    public Vector3 minscale;
    public Vector3 maxscale;
    public AudioLoudnessDetector detector;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip);

        transform.localScale = Vector3.Lerp(minscale, maxscale, loudness);
    }
}
