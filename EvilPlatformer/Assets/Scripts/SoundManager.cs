using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleSound
{
    public string key;       // e.g. "Jump", "Coin", etc.
    public AudioClip clip;   // Assign via Inspector
}

public class SoundManager : MonoBehaviour
{ 
    // Singleton Instance
    public static SoundManager Instance { get; private set; }

    [Header("Assign one AudioSource for SFX")]
    public AudioSource sfxSource;

    [Header("List your sounds here (key + clip)")]
    public List<SimpleSound> sounds = new List<SimpleSound>();

    // Internal lookup table
    private Dictionary<string, AudioClip> _clipLookup;

    private void Awake()
    {
        // Enforce singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build a simple lookup dictionary
        _clipLookup = new Dictionary<string, AudioClip>();
        foreach (var s in sounds)
        {
            if (s.clip != null && !string.IsNullOrEmpty(s.key))
                _clipLookup[s.key] = s.clip;
        }

        // If no AudioSource was assigned, add one on the fly
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Play a one-shot sound by its key.
    /// </summary>
    public void PlaySound(string key, float volume = 1f)
    {
        if (_clipLookup == null || !_clipLookup.ContainsKey(key))
        {
            Debug.LogWarning($"[SoundManager] No sound found for key: {key}");
            return;
        }

        AudioClip clip = _clipLookup[key];

        sfxSource.pitch = Random.Range(.93f, 1.07f);

        sfxSource.PlayOneShot(clip, volume);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController2D>())
        { 
            //do something
        }
    }
}
