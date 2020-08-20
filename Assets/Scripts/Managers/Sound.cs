using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
