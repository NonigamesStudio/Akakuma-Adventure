using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

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
    [SerializeField] List <GameObject>  shields;
    [SerializeField] PlayerInventoryUIManager playerInventoryUIManager;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] Camera cam;
    private RaycastHit hit;
    private Interactable interactingObject=null;
    [SerializeField] private LayerMask interactableLayer;
    

    
    [Header("Player Variables")]
    [SerializeField] GameObject currentWeaponFirstHandObj;
    [SerializeField] GameObject currentWeaponSecondHandObj;
    public float bonusDamageToCharge;
    public float coolDownDashTime;
    
    private Coroutine coroutine;

   

    IWeapon currentWeaponFirstHand;
    IWeapon currentWeaponSecondHand;

    public bool onSkill;

    public float chargeTime;
    public bool boolCharging=false;
    public bool shooting;
    bool canClick=true;
    public bool isStuned=false;
    bool usingyWeapon;

    public System.Action OnWeaponSkill;
    public System.Action OnWeaponAttack;
    public System.Action OnBowRealese;
    public System.Action OnBowReady;
    public System.Action OnSCPPress;
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
        if (cam == null) cam = Camera.main;
    }

    void OnDeathPlayer()
    {
        GameController.instances.OnEndGame();
    }

    private void Update()//Input Detection
    {
        InventoryInput();
        if (isStuned) return;
        AttackSecondaryWeaponInput();
        playerMovement.Rotate();
        //MovementInput();
        DashInput();
        if (onSkill) return;
        AttackPrincipalWeaponInput();

        
        
    }
    private void FixedUpdate()
    {
        if (isStuned) return;
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

    public void TakeBackSword()
    {
        currentWeaponFirstHand = inventory.GetWeaponSelected(0);
        foreach (GameObject shield in shields)
        { shield.SetActive(false); }
        uIManager.ChangeWeaponSpriteAbility(0);
        if (currentWeapon != 0)
        {
            currentWeapon = 0;
            PlayWeaponChangeAnimation(AnimNamesPlayer.DrawSword);
            PlayWeaponChangeSound();
        }
    }
    private void AttackPrincipalWeaponInput()
    {
        if (usingyWeapon) return;
        if (!canClick) return;
        if (Input.GetMouseButtonDown(0)) usingyWeapon = false;
        if (Input.GetMouseButton(0)) chargeTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(0))
        {
            Ray rayMouse = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(rayMouse, out RaycastHit hit,100,interactableLayer))
            {
                this.hit = hit;
            }
            int interactionDistance = 3;
            if (hit.transform != null && Vector3.Distance(hit.transform.position, this.transform.position)<interactionDistance && hit.transform.TryGetComponent(out Interactable interactable))
            {
                interactable.Interact();
                if (interactable.interactableType == InteractableType.Shop)
                {
                    
                    GetStuned();
                    interactable.OnCloseInteraction += CloseUIInteraction;
                    interactingObject = interactable;
                }
            }else{

            currentWeaponFirstHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
            chargeTime = 0;
            usingyWeapon = false;
            }
            
            
        }
    }
    
    public void InventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!playerInventoryUIManager.isUIOpen)
            {
            inventoryUI.gameObject.SetActive(true);
            playerInventoryUIManager.UpdateInventory();
            playerInventoryUIManager.isUIOpen=true;
            GetStuned();
            
            }else
            {
            inventoryUI.gameObject.SetActive(false);
            playerInventoryUIManager.isUIOpen=false;
            RemoveStun();
            }
        }


        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // currentWeaponFirstHand = inventory.GetWeaponSelected(0);
            // foreach (GameObject shield in shields)
            // {shield.SetActive(false);}
            // uIManager.ChangeWeaponSpriteAbility(0);
            // if (currentWeapon!=0) 
            // {
            
            // currentWeapon = 0;
            // PlayWeaponChangeAnimation(AnimNamesPlayer.DrawSword);
            // PlayWeaponChangeSound();
            // }
            if (TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
            {
                int slot = 21;
                try {
                playerInventory.UseItem(slot);
                }catch{
              
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
            {
                int slot = 20;
                try {
                playerInventory.UseItem(slot);
                }catch{
               
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUIInteraction();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(1);
            foreach (GameObject shield in shields)
            {shield.SetActive(false);}
            uIManager.ChangeWeaponSpriteAbility(2);
            if (currentWeapon!=1) 
            {
            currentWeapon = 1;
            PlayWeaponChangeSound();
              
            }
            currentWeaponFirstHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
            chargeTime = 0;
            usingyWeapon = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeaponFirstHand = inventory.GetWeaponSelected(2);
            foreach (GameObject shield in shields)
            {shield.SetActive(true);}
            uIManager.ChangeWeaponSpriteAbility(4);
            if (currentWeapon!=2) 
            {
            currentWeapon = 2;
            PlayWeaponChangeSound();
            }
            currentWeaponFirstHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
            chargeTime = 0;
            usingyWeapon = false;
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
    
    private void AttackSecondaryWeaponInput()
    {
        chargeTime += Time.deltaTime;

       

        if (Input.GetMouseButton(1)) 


        if (isStuned) return;
        
        if (Input.GetMouseButtonUp(1))
        {
            boolCharging = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!shooting) shooting = true;
            else return;
            boolCharging = false;
            chargeTime = 0;
            playerMovement.SlowDown(true);
            usingyWeapon = true;
            currentWeaponFirstHand.TurnOnOffWeapon(false);
            currentWeaponSecondHand.TurnOnOffWeapon(true);
            AnimController_Player.ins.PlayAnim(AnimNamesPlayer.AttackBow);
            OnBowReady?.Invoke();
        }


        if (chargeTime < 0.6f)  return;
        
        if (boolCharging) { 
            BowRealese();
            return; 
        }


    }

    public void GetStuned(float sec=0)
    {
        rb.velocity = Vector2.zero;
        isStuned = true;
        if (sec == 0) return;
        LeanTween.delayedCall(sec, () => { isStuned = false; });
    }
    public void RemoveStun(float afterSeconds=0)
    {
        LeanTween.delayedCall(afterSeconds, () => { 
        rb.velocity = Vector2.zero;
        isStuned = false;
        if (interactingObject != null)
        {
            interactingObject.OnCloseInteraction -= CloseUIInteraction;
            interactingObject = null;
        } });
        
    }
    private void PlayWeaponChangeSound()
    {
       if(AudioManager.instance !=null) AudioManager.instance.PlayOneShot(FMODEvents.instance.changeWeapon, t.position);
    }
    private void PlayWeaponChangeAnimation(AnimNamesPlayer animNamesPlayer)
    {
       AnimController_Player.ins.PlayAnim(animNamesPlayer);
    }
    // IEnumerator RepositionBowCorrutine(float elapsedTime)
    // {
    //     yield return new WaitForSeconds(0.5f-elapsedTime);
    //     BowRealese();
    // }
    private void BowRealese()
    {
        currentWeaponSecondHand.Attack(Mathf.Round(stats.attack + chargeTime * bonusDamageToCharge));
        AnimController_Player.ins.PlayAnim(AnimNamesPlayer.ReleaseBow);
        playerMovement.SlowDown(false);
        chargeTime = 0;
        usingyWeapon = false;
        currentWeaponFirstHand.TurnOnOffWeapon(true);
        currentWeaponSecondHand.TurnOnOffWeapon(false);
        boolCharging=false;
        shooting = false ;
    }
    public void CloseUIInteraction()
    {
        if (playerInventoryUIManager.isUIOpen)
        {
        inventoryUI.gameObject.SetActive(false);
        playerInventoryUIManager.isUIOpen=false;
        }
        StartCoroutine(CallCloseInteraction(0.1f));
       OnSCPPress?.Invoke();
    }

    private IEnumerator CallCloseInteraction(float sec)
    {
        yield return new WaitForSeconds(sec);
        RemoveStun();   
        
    }
    public void DisableClick()
    {
        canClick = false;
    }
    public void EnableClick()
    {
        canClick = true;
    }


   

}
