using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Transform t;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Health healthPlayer;
    [HideInInspector] public Stats stats;
    [HideInInspector] public WeaponInventory inventory;
    [HideInInspector] public Collider playerCollider;
   


    [Header("Player Variables")]
    [SerializeField] GameObject currentWeaponFirstHandObj;
    [SerializeField] GameObject currentWeaponSecondHandObj;
    public float bonusDamageToCharge;
    public float coolDownDashTime;


     IWeapon currentWeaponFirstHand;
     IWeapon currentWeaponSecondHand;


    bool coolDownDash = true;
    float chargeTime;
    bool isStuned;
    bool usingyWeapon;

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

    private void Update()//Input Detection
    {
        
        if (isStuned) return;
        playerMovement.Rotate();
        MovementInput();
        DashInput();
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
            if (!coolDownDash) { Debug.Log("Dash on Cooldown"); return; }
            GetStuned(playerMovement.speedDash);
            coolDownDash = false;
            LeanTween.delayedCall(coolDownDashTime, () => { coolDownDash = true; });
            playerMovement.DashMove();
        }
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) playerMovement.Run();
        else playerMovement.Walk();
    }

    private void InventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeaponFirstHand = inventory.GetWeaponSelected(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeaponFirstHand = inventory.GetWeaponSelected(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeaponFirstHand = inventory.GetWeaponSelected(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentWeaponFirstHand = inventory.GetWeaponSelected(3);
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
            usingyWeapon = true;
            currentWeaponFirstHand.TurnOnOffWeapon(false);
            currentWeaponSecondHand.TurnOnOffWeapon(true);
        }
        if (Input.GetMouseButton(1)) chargeTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(1))
        {
            currentWeaponSecondHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
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

}
