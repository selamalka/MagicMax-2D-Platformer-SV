using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerSFX;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] AudioClip[] footsteps;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }
}
