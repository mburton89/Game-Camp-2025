using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SimpleSoundTestEntry
{
    [Tooltip("The key (string) matching an entry in your SoundManager's 'sounds' list.")]
    public string soundKey;

    [Tooltip("The keyboard key that will trigger this sound.")]
    public KeyCode keyToPress;

    [Tooltip("Playback volume (0–1) for this test.")]
    [Range(0f, 1f)]
    public float volume;
}

/// <summary>
/// Attach this to any GameObject in your scene. Populate the 'tests' list with pairs of
/// (soundKey, KeyCode). At runtime, pressing the specified KeyCode will call:
///     SoundManager.Instance.PlaySound(soundKey, volume);
/// </summary>
public class SoundTester : MonoBehaviour
{
    [Header("Assign a list of sound tests below")]
    public List<SimpleSoundTestEntry> tests = new List<SimpleSoundTestEntry>();

    void Update()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("[SimpleSoundManagerTester] No SoundManager.Instance found in scene.");
            return;
        }

        // Iterate through each entry in 'tests' and check for key presses
        foreach (var entry in tests)
        {
            if (Input.GetKeyDown(entry.keyToPress))
            {
                SoundManager.Instance.PlaySound(entry.soundKey, entry.volume);
            }
        }
    }
}
