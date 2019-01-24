using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    //***** Generation does not appear to work in WEBGL *****
    //[Range(1, 20000)]  //Creates a slider in the inspector
    //public float frequency1=440f;

    //[Range(1, 20000)]  //Creates a slider in the inspector
    //public float frequency2=440f;

    //public float sampleRate = 44100;
    //public float waveLengthInSeconds = 1.0f;

    AudioSource audioSource;
    int timeIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.playOnAwake = false;
        //audioSource.spatialBlend = 0; //force 2D sound
        //audioSource.Stop(); //avoids audiosource from starting to play automatically
        //Generate();

    }

    public void Play()
    {
        audioSource.Play(); 
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    //void OnAudioFilterRead(float[] data, int channels)
    //{
    //    for (int i = 0; i < data.Length; i += channels)
    //    {
    //        data[i] = CreateSine(timeIndex, frequency1, sampleRate);

    //        if (channels == 2)
    //            data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

    //        timeIndex++;

    //        //if timeIndex gets too big, reset it to 0
    //        if (timeIndex >= (sampleRate * waveLengthInSeconds))
    //        {
    //            timeIndex = 0;
    //        }
    //    }
    //}
    ////Creates a sinewave
    //public float CreateSine(int timeIndex, float frequency, float sampleRate)
    //{
    //    return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    //}

    //void Generate()
    //{
    //    float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
    //    audioSource.clip.GetData(samples, 0);
    //    int Length= samples.Length;
    //    if (audioSource.clip.channels == 2)
    //        Length -= 1;
    //        for (int i = 0; i < Length; ++i)
    //    {
    //        samples[i] = CreateSine(timeIndex, frequency1, sampleRate);
    //        if (audioSource.clip.channels == 2)
    //            samples[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

    //        timeIndex++;

    //        //if timeIndex gets too big, reset it to 0
    //        if (timeIndex >= (sampleRate * waveLengthInSeconds))
    //        {
    //            timeIndex = 0;
    //        }
    //        //samples[i] = samples[i] * 0.5f;
    //    }

    //    audioSource.clip.SetData(samples, 0);
    //}
}
