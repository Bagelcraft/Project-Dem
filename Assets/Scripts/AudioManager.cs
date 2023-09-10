using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // Name of the sound.
    public AudioClip clip; // Audio clip to play.
    [Range(0f, 1f)]
    public float volume = 1f; // Volume of the sound.
    [Range(0.1f, 3f)]
    public float pitch = 1f; // Pitch of the sound.
    public bool loop = false; // Should the sound loop?
    [HideInInspector]
    public AudioSource source; // Reference to the AudioSource component.
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance.

    public Sound[] sounds; // Array to store your audio clips.

    void Awake()
    {
        // Implement the Singleton pattern.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager between scenes.
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound soundToPlay = System.Array.Find(sounds, sound => sound.name == name);
        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }

        soundToPlay.source.Play();
    }

    // Add other methods like StopSound, PauseSound, etc., as needed.

    // Example usage: AudioManager.instance.PlaySound("YourSoundName");
}
