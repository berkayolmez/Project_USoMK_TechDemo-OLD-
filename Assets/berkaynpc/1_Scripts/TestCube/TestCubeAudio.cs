using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class TestCubeAudio : MonoBehaviour ,IHaveSignal
    {
        public int position = 0;
        [Range(30000, 100000)]
        public int samplerate = 44100;
        [Range(0,700)]
        public float frequency = 440;

        void Start()
        {
            AudioClip myClip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
            AudioSource aud = GetComponent<AudioSource>();
            aud.volume = 0.02f;
            aud.clip = myClip;
            aud.Play();
        }

        void OnAudioRead(float[] data)
        {
            int count = 0;
            while (count < data.Length)
            {
                data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
                position++;
                count++;
            }
        }

        void OnAudioSetPosition(int newPosition)
        {
            position = newPosition;
        }

        public void GetSignal(float getSignal,float maxSignal)
        {
           // frequency = getSignal;
        }
    }
}