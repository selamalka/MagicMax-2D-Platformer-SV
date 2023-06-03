using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    [field: SerializeField] public Transform NimbusInstancesParent { get; private set; }

    [SerializeField] private Transform body;
    [SerializeField] private Transform projectileOriginSide;
    [SerializeField] private Transform projectileOriginTop;
    [SerializeField] private Transform projectileOriginBottom;
    [SerializeField] private GameObject meleeSlashPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [field: SerializeField] public SpellSlot SpellSlot1 { get; private set; }
    [field: SerializeField] public SpellSlot SpellSlot2 { get; private set; }
    [SerializeField] private float meleeCooldownTime;
    private float meleeCooldownCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        meleeCooldownCounter = 0;

        if (GameObject.Find("Spell Slot 1") == null)
        {
            SpellSlot1 = GameObject.Find("Spell Slot 1").GetComponent<SpellSlot>();
            SpellSlot2 = GameObject.Find("Spell Slot 2").GetComponent<SpellSlot>();
        }
    }

    private void Update()
    {
        if (meleeCooldownCounter > 0)
        {
            meleeCooldownCounter -= Time.deltaTime;
        }

        if (!PlayerController.Instance.IsControllable) return;

        if (InputManager.Instance.IsEPressed())
        {
            //MeleeSlash();
            PlayerController.Instance.Animator.SetTrigger("meleeAttack");
            AudioManager.Instance.PlayMeleeCast();
        }

        if (InputManager.Instance.IsQPressed())
        {
            UseSpellSlot1();
        }

        if (InputManager.Instance.IsWPressed())
        {
            UseSpellSlot2();
        }
    }

    private void UseSpellSlot1()
    {
        if (SpellSlot1.CurrentSpell == null) return;

        else if (PlayerStatsManager.Instance.CurrentMana == 0) print("Not enough mana");
        else
        {
            var spellContainer = Instantiate(SpellSlot1.CurrentSpell.SpellPrefab, projectileOriginSide.position, Quaternion.identity);
            spellContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);
            PlayerController.Instance.Animator.SetTrigger("castSpell");
            if (SpellSlot1.CurrentSpell.SpellData.AudioClip != null)
            {
                AudioManager.Instance.PlayCastSpell(SpellSlot1.CurrentSpell.SpellData.AudioClip);
            }
            if (PlayerStatsManager.Instance.IsEnoughMana(SpellSlot1.CurrentSpell.SpellData.ManaCost))
            {
                PlayerStatsManager.Instance.UseMana(SpellSlot1.CurrentSpell.SpellData.ManaCost);
            }
            else
            {
                Destroy(spellContainer);
                print("not enough mana");
            }
        }
    }
    private void UseSpellSlot2()
    {
        if (SpellSlot2.CurrentSpell == null) return;

        else if (PlayerStatsManager.Instance.CurrentMana == 0) print("Not enough mana");
        else
        {
            var spellContainer = Instantiate(SpellSlot2.CurrentSpell.SpellPrefab, projectileOriginSide.position, Quaternion.identity);
            spellContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);
            PlayerController.Instance.Animator.SetTrigger("castSpell");
            if (SpellSlot2.CurrentSpell.SpellData.AudioClip != null)
            {
                AudioManager.Instance.PlayCastSpell(SpellSlot2.CurrentSpell.SpellData.AudioClip);
            }
            if (PlayerStatsManager.Instance.IsEnoughMana(SpellSlot2.CurrentSpell.SpellData.ManaCost))
            {
                PlayerStatsManager.Instance.UseMana(SpellSlot2.CurrentSpell.SpellData.ManaCost);
            }
            else
            {
                Destroy(spellContainer);
                print("not enough mana");
            }
        }
    }
    public void LoadSpellSlotsInfo()
    {
        SpellSlot1.LoadSpellSlotInfo(1);
        SpellSlot2.LoadSpellSlotInfo(2);
    }

    //Animation Event
    public void MeleeSlash()
    {
        if (meleeCooldownCounter <= 0)
        {
            if (PlayerController.Instance.IsGrounded && Input.GetKey(KeyCode.DownArrow)) return;
            if (PlayerController.Instance.IsNimbusActive && Input.GetKey(KeyCode.DownArrow)) return;

            var origin = GetSpellOrigin();
            var meleeInstance = Instantiate(meleeSlashPrefab, GetSpellOrigin(), Quaternion.identity);

            meleeInstance.transform.localScale = body.localScale;

            if (origin == projectileOriginTop.position)
            {
                meleeInstance.transform.rotation = PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
            }
            else if (origin == projectileOriginBottom.position)
            {
                meleeInstance.transform.rotation = PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
            }
            meleeCooldownCounter = meleeCooldownTime;
        }
    }

    public void InstantiateHitEffect(Vector3 origin, Transform enemyTransform)
    {
        var enemyColliderCenter = enemyTransform.GetComponent<Collider2D>().bounds.center;
        var hitEffect = Instantiate(hitEffectPrefab, enemyColliderCenter, PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0));

        if (origin == projectileOriginTop.position)
        {
            hitEffect.transform.rotation *= PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, 60) : Quaternion.Euler(0, 0, 60);
        }
        else if (origin == projectileOriginBottom.position)
        {
            hitEffect.transform.rotation *= PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, -70) : Quaternion.Euler(0, 0, -70);
        }

        var newHitScale = hitEffect.transform.localScale;
        newHitScale = new Vector3(newHitScale.x + Random.Range(-0.1f, 0.2f), newHitScale.y + Random.Range(-0.1f, 0.2f), newHitScale.z);
        hitEffect.transform.localScale = newHitScale;
    }

    private Vector3 GetSpellOrigin()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                return projectileOriginTop.position;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                return projectileOriginBottom.position;
            }

            return projectileOriginSide.position;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            return projectileOriginTop.position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            return projectileOriginBottom.position;
        }

        else return projectileOriginSide.position;
    }
}
