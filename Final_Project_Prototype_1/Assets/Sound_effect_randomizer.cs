using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sound_effect_randomizer : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void play()
    {
        if (clips.Length == 0)
        {
            print("no clips");
            return;
        }

        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

}
