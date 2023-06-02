using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerSFX;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] AudioClip[] footsteps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlayRandomFootstep()
    {
        playerSFX.volume = 0.4f;
        playerSFX.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }
}
