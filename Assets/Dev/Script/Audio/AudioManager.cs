using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public EventInstance musicEventInstance;
    
    void Start()
    {
        
    }

    void Awake()
    {
        instance = this;
    }
    public void PlayOneShot (EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayMusic (EventReference music)
    {
        musicEventInstance = RuntimeManager.CreateInstance(music);
        musicEventInstance.start();
    }
    public void SetMenuMusicEnd()
    {
        musicEventInstance.setParameterByName("End", 1);
    }
  
}
