using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerSFX;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip turnWoosh;
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] landOnGround;

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

    public void PlayRandomLandOnGround()
    {
        playerSFX.volume = 0.5f;
        playerSFX.PlayOneShot(landOnGround[Random.Range(0, landOnGround.Length)]);
    }

    public void PlayTurnWoosh()
    {
        playerSFX.volume = 0.25f;
        playerSFX.PlayOneShot(turnWoosh);
    }
}
