using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public static PlayerStats Instance;

    [field: SerializeField] public float InvulnerableStartTime { get; private set; }
    [SerializeField] private bool isInvulnerable;
    private float invulnerableCounter;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        InvulnerableHandler();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Thorns"))
        {
            if (!isInvulnerable)
            {
                TakeDamageOnEnemyCollision(collision);
            }
        }
    }

    private void InvulnerableHandler()
    {
        if (!isInvulnerable) return;

        Physics2D.IgnoreLayerCollision(7, 8);

        if (invulnerableCounter > 0)
        {
            invulnerableCounter -= Time.deltaTime;
            FXManager.Instance.ChangePlayerTransparency(0.4f);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(7, 8, false);
            isInvulnerable = false;
            invulnerableCounter = InvulnerableStartTime;
            FXManager.Instance.ChangePlayerTransparency(1f);
        }
    }

    public async void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        PlayerController.Instance.Animator.SetTrigger("getHit");
        AudioManager.Instance.PlayPlayerGetHit();
        isInvulnerable = true;
        invulnerableCounter = InvulnerableStartTime;
        PlayerController.Instance.Knockback(PlayerController.Instance.IsFacingRight ? Vector2.right : Vector2.left, 15, 15, 250);
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
        ProgressionManager.Instance.Progression.SetHealthPoints(PlayerStatsManager.Instance.CurrentHealth);
        FXManager.Instance.FlashWhite(gameObject);
        FXManager.Instance.PauseGameEffect(100);

        if (FXManager.Instance.PlayerHitShakePreset != null)
        {
            FXManager.Instance.CameraShaker.Shake(FXManager.Instance.PlayerHitShakePreset); 
        }
        EventManager.OnPlayerGetHit?.Invoke();

        if (PlayerStatsManager.Instance.CurrentHealth <= 0)
        {            
            Destroy(gameObject);
            ProgressionManager.Instance.Progression.SetHealthPoints(5);
            ProgressionManager.Instance.Progression.SetManaPoints(0);
            ProgressionManager.Instance.Progression.SetSoulPoints(0);
            ProgressionManager.Instance.Progression.SetSpellPoints(PlayerStatsManager.Instance.SpellPoints);
            GameManager.Instance.ResumeGame();
            UIManager.Instance.FadeToBlack(2);
            await Task.Delay(2000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void TakeDamageOnEnemyCollision(Collision2D collision)
    {
        Vector2 enemyDirection = collision.gameObject.transform.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (!collision.gameObject.CompareTag("Behemoth"))
        {
            TakeDamage(1);            
        }
    }
}
