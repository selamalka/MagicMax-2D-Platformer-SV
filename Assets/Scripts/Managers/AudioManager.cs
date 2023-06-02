using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource enemySFXAudioSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip turnWoosh;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip impDeath;
    [SerializeField] private AudioClip[] impGetHit;
    [SerializeField] private AudioClip[] enemyImpact;
    [SerializeField] private AudioClip[] meleeCast;
    [SerializeField] private AudioClip[] footstep;
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

    public void PlayFootstep()
    {
        sfxAudioSource.volume = 0.4f;
        sfxAudioSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
    }

    public void PlayLandOnGround()
    {
        sfxAudioSource.volume = 0.5f;
        sfxAudioSource.PlayOneShot(landOnGround[Random.Range(0, landOnGround.Length)]);
    }

    public void PlayTurnWoosh()
    {
        sfxAudioSource.volume = 0.1f;
        sfxAudioSource.PlayOneShot(turnWoosh);
    }

    public void PlayJump()
    {
        sfxAudioSource.volume = 0.3f;
        sfxAudioSource.PlayOneShot(jump);
    }

    public void PlayMeleeCast()
    {
        sfxAudioSource.volume = 0.5f;
        sfxAudioSource.PlayOneShot(meleeCast[Random.Range(0,meleeCast.Length)]);
    }

    public void PlayEnemyImpact()
    {
        sfxAudioSource.volume = 0.4f;
        sfxAudioSource.PlayOneShot(enemyImpact[Random.Range(0, enemyImpact.Length)]);
    }

    public void PlayImpGetHit()
    {
        enemySFXAudioSource.volume = 0.3f;
        enemySFXAudioSource.PlayOneShot(impGetHit[Random.Range(0, impGetHit.Length)]);
    }

    public void PlayImpDeath()
    {
        enemySFXAudioSource.volume = 0.3f;
        enemySFXAudioSource.PlayOneShot(impDeath);
    }
}
