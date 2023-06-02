using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerSFXAudioSource;
    [SerializeField] private AudioSource enemySFXAudioSource;
    [SerializeField] private AudioSource sceneSFXAudioSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip turnWoosh;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip playerGetHit;
    [SerializeField] private AudioClip powerupDrop;
    [SerializeField] private AudioClip pickupPowerup;
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
        playerSFXAudioSource.volume = 0.4f;
        playerSFXAudioSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
    }

    public void PlayLandOnGround()
    {
        playerSFXAudioSource.volume = 0.7f;
        playerSFXAudioSource.PlayOneShot(landOnGround[Random.Range(0, landOnGround.Length)]);
    }

    public void PlayTurnWoosh()
    {
        playerSFXAudioSource.volume = 0.1f;
        playerSFXAudioSource.PlayOneShot(turnWoosh);
    }

    public void PlayJump()
    {
        playerSFXAudioSource.volume = 0.3f;
        playerSFXAudioSource.PlayOneShot(jump);
    }

    public void PlayDash()
    {
        playerSFXAudioSource.volume = 0.3f;
        playerSFXAudioSource.PlayOneShot(dash);
    }

    public void PlayPlayerGetHit()
    {
        playerSFXAudioSource.volume = 0.7f;
        playerSFXAudioSource.PlayOneShot(playerGetHit);
    }

    public void PlayMeleeCast()
    {
        playerSFXAudioSource.volume = 0.5f;
        playerSFXAudioSource.PlayOneShot(meleeCast[Random.Range(0,meleeCast.Length)]);
    }

    public void PlayEnemyImpact()
    {
        playerSFXAudioSource.volume = 0.7f;
        playerSFXAudioSource.PlayOneShot(enemyImpact[Random.Range(0, enemyImpact.Length)]);
    }

    public void PlayImpGetHit()
    {
        enemySFXAudioSource.volume = 0.2f;
        enemySFXAudioSource.PlayOneShot(impGetHit[Random.Range(0, impGetHit.Length)]);
    }

    public void PlayImpDeath()
    {
        enemySFXAudioSource.volume = 0.3f;
        enemySFXAudioSource.PlayOneShot(impDeath);
    }

    public void PlayPowerupDrop()
    {
        sceneSFXAudioSource.volume = 0.7f;
        sceneSFXAudioSource.PlayOneShot(powerupDrop);
    }

    public void PlayPickupPowerup()
    {
        sceneSFXAudioSource.volume = 0.5f;
        sceneSFXAudioSource.PlayOneShot(pickupPowerup);
    }
}
