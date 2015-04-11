using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sound_effect_randomizer : MonoBehaviour
{
    public AudioClip[] clips;
    public bool allow_overlap = true;

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

        if (source.isPlaying)
        {
            print("already playing");
            return;
        }

        if (allow_overlap)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            return;
        }

        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }

}
