using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    [Range(0,2f)]
    public float volume;
    [Range(-2f,2f)]
    public float pitch;
    public string name;
        
    [HideInInspector]
    public AudioSource source;
}
