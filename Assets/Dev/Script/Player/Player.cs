using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event EventHandler OnDash;
    [HideInInspector] public Transform t;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Health healthPlayer;
    [HideInInspector] public Stats stats;
    [HideInInspector] public WeaponInventory inventory;
    [HideInInspector] public Collider playerCollider;

    [SerializeField] UIManager uIManager;
    [SerializeField] GameObject shieldR;

    [Header("Player Variables")]
    [SerializeField] GameObject currentWeaponFirstHandObj;
    [SerializeField] GameObject currentWeaponSecondHandObj;
    public float bonusDamageToCharge;
    public float coolDownDashTime;


    IWeapon currentWeaponFirstHand;
    IWeapon currentWeaponSecondHand;

    public bool onSkill;

    float chargeTime;
    [HideInInspector] public bool isStuned;
    bool usingyWeapon;

    public System.Action OnWeaponSkill;
    public System.Action OnWeaponAttack;
    public System.Action OnBowRealese;
    public System.Action OnBowReady;
    public int currentWeapon;

    private void OnEnable()
    {
        healthPlayer.OnDeath += OnDeathPlayer;
    }
    private void OnDisable()
    {
        healthPlayer.OnDeath -= OnDeathPlayer;
    }
    private void Start()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        healthPlayer = GetComponent<Health>();
        stats = GetComponent<Stats>();
        inventory = GetComponent<WeaponInventory>();
        playerCollider = GetComponent<Collider>();

        currentWeaponFirstHand = currentWeaponFirstHandObj.GetComponent<IWeapon>();
        currentWeaponSecondHand = currentWeaponSecondHandObj.GetComponent<IWeapon>();


    }

    void OnDeathPlayer()
    {
        GameController.instances.OnEndGame();
    }

    private void Update()//Input Detection
    {
        
        if (isStuned) return;
        playerMovement.Rotate();
        MovementInput();
        DashInput();
        if (onSkill) return;
        AttackPrincipalWeaponInput();
        SkillPrincipalWeaponInput();
        AttackSecondaryWeaponInput();
        InventoryInput();
        
    }
    private void FixedUpdate()
    {
        MovementInput();
    }

    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDash?.Invoke(this, EventArgs.Empty);
        }
            
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) playerMovement.Run();
        else playerMovement.Walk();
    }

    private void InventoryInput()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(0);
            shieldR.SetActive(false);
            uIManager.ChangeWeaponSpriteAbility(0);
            if (currentWeapon!=0) 
            {
            
            currentWeapon = 0;
            PlayWeaponChangeAnimation(AnimNamesPlayer.DrawSword);
            PlayWeaponChangeSound();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(1);
            shieldR.SetActive(false);
            uIManager.ChangeWeaponSpriteAbility(2);
            if (currentWeapon!=1) 
            {
            currentWeapon = 1;
            PlayWeaponChangeSound();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(2);
            shieldR.SetActive(true);
            uIManager.ChangeWeaponSpriteAbility(4);
            if (currentWeapon!=2) 
            {
            currentWeapon = 2;
            PlayWeaponChangeSound();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(3);
            uIManager.ChangeWeaponSpriteAbility(6);
            if (currentWeapon!=3) 
            {
            currentWeapon = 3;
            PlayWeaponChangeSound();
            }
        }
            
    }

    private void SkillPrincipalWeaponInput()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            currentWeaponFirstHand.Skill();
        }
    }
    private void AttackPrincipalWeaponInput()
    {
        if (usingyWeapon) return;
        if (Input.GetMouseButtonDown(0)) usingyWeapon = false;
        if (Input.GetMouseButton(0)) chargeTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(0))
        {
            currentWeaponFirstHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
            chargeTime = 0;
            usingyWeapon = false;
        }
    }
    private void AttackSecondaryWeaponInput()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            playerMovement.SlowDown(true);
            usingyWeapon = true;
            currentWeaponFirstHand.TurnOnOffWeapon(false);
            currentWeaponSecondHand.TurnOnOffWeapon(true);
            AnimController_Player.ins.PlayAnim(AnimNamesPlayer.AttackBow);
            OnBowReady?.Invoke();
        }
        if (Input.GetMouseButton(1)) chargeTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(1))
        {
            currentWeaponSecondHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
            playerMovement.SlowDown(false);
            chargeTime = 0;
            usingyWeapon = false;
            currentWeaponFirstHand.TurnOnOffWeapon(true);
            currentWeaponSecondHand.TurnOnOffWeapon(false);
            
        }
    }

    public void GetStuned(float sec)
    {
        rb.velocity = Vector2.zero;
        isStuned = true;
        LeanTween.delayedCall(sec, () => { isStuned = false; });
    }
    private void PlayWeaponChangeSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.changeWeapon, t.position);
    }
    private void PlayWeaponChangeAnimation(AnimNamesPlayer animNamesPlayer)
    {
       AnimController_Player.ins.PlayAnim(animNamesPlayer);
    }

}
