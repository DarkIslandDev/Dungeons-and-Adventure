using System;
using UnityEngine;

public enum ControlSchemeType
{
    Keyboard,
    Gamepad
}

public class PlayerManager : MonoBehaviour
{
    #region Instance

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("у тебя тут где-то ещё один такой же скрипт. Найди его быстрее и удали к хуям собачим, а может быть лишний скрипт это Я.");
            return;
        }

        instance = this;
    }

    #endregion
    
    [HideInInspector]
    public InputHandler inputHandler;
    [HideInInspector] 
    public CameraHandler cameraHandler;
    private Animator animator;
    [HideInInspector] 
    public PlayerLocomotion playerLocomotion;
    [HideInInspector] 
    public UIManager UIManager;

    public ControlSchemeType controlSchemeType;

    public GameObject interactableUIGameObject;
    [HideInInspector] public string interactableText;


    public bool isInteracting;

    [Header("Player Flags")] public bool isSprinting;
    public bool isWalking;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        UIManager = UIManager.instance;
        isInteracting = false;

        cameraHandler = FindObjectOfType<CameraHandler>();

        // interactableUIGameObject = UIManager.instance.interactableUI.popUpAsGameObject;
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("CanDoCombo");
        animator.SetBool("isInAir", isInAir);

        inputHandler.TickInput(delta);
        inputHandler.HandleAttackInput(delta);
        inputHandler.HandleQuickSlotsInput();
        inputHandler.HandleJumpInput();
        
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();

        CheckForInteractable();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;


        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.latk_Input = false;
        inputHandler.interactable_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.menu_Input = false;
        inputHandler.inventory_Input = false;
        
        inputHandler.first_slot = false;
        inputHandler.second_slot = false;
        inputHandler.third_slot = false;
        inputHandler.fourth_slot = false;
        
        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }

    public void CheckForInteractable()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f,
                transform.forward, out hit,
                1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    // interactableText = interactableObject.interactableText;
                    UIManager.HUD.interactableUI.interactableText.text = interactableObject.interactableText;
                    // UIManager.instance.interactableUI.itemIcon.sprite = interactableObject.itemIcon.sprite;
                    UIManager.HUD.interactableUI.popUpAsGameObject.SetActive(true);

                    if (inputHandler.interactable_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }                                               /////       переделать к хуям загрузил как ебанафт
        else
        {
            if (UIManager.HUD.interactableUI.popUpAsGameObject != null)
            {
                UIManager.HUD.interactableUI.popUpAsGameObject.SetActive(false);
            }

            if (UIManager.HUD.interactableUI.popUpAsGameObject != null && inputHandler.interactable_Input)
            {
                UIManager.HUD.interactableUI.popUpAsGameObject.SetActive(false);
            }
        }
    }

    public void CheckForControlScheme()
    {
        switch (controlSchemeType)
        {
            case ControlSchemeType.Keyboard:
                break;
            case ControlSchemeType.Gamepad:
                break;
        }
    }
}