using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool rs_Input;
    public bool latk_Input;
    public bool hatk_Input;
    public bool interactable_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool menu_Input;

    public bool first_slot;
    public bool second_slot;
    public bool third_slot;
    public bool fourth_slot;
    
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public bool menuFlag;
    
    public float rollInputTimer;

    private PlayerControls inputActions;
    [HideInInspector]
    public PlayerManager playerManager;
    public PlayerAttacker playerAttacker;
    public PlayerInventory inventory;

    private Vector2 movementInput;
    private Vector2 cameraInput;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        inventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed +=
                inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }
        
        inputActions.PlayerActions.ZoomCamera.performed += HandleCameraZoom;
        
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleInteractableButtonInput();
        HandleMenuInput();
        HandleInventoryInput();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal)+ Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        rs_Input = inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;
        sprintFlag = rs_Input;
        
        if(rs_Input)
        {
            rollInputTimer += delta;
        }
        else
        {
            if(rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;

        }
    }

    public void HandleAttackInput(float delta)
    {
        if (inventory.rightWeapon != inventory.unarmedWeapon)
        {
            if (playerManager.isInteracting == false)
            {
                inputActions.PlayerActions.LightAttack.started += i => latk_Input = true;
                // inputActions.PlayerActions.HeavyAttack.started += i => hatk_Input = true;

                if (latk_Input)
                {
                    if (playerManager.canDoCombo)
                    {
                        comboFlag = true;
                        playerAttacker.HandleLightAttack(inventory.rightWeapon);
                        comboFlag = false;
                    }
                    else
                    {
                        if (playerManager.canDoCombo)
                        {
                            return;
                        }
                    
                        playerAttacker.HandleLightAttack(inventory.rightWeapon);
                    }
                }

                // if (hatk_Input)
                // {
                //     playerAttacker.HandleHeavyAttack(inventory.rightWeapon);
                // }
            }
        }
        else
        {
            playerManager.isInteracting = false;
        }
    }

    public void HandleCameraZoom(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            playerManager.cameraHandler.Zooming(value);
        }
    }

    public void HandleQuickSlotsInput()
    {
        inputActions.QuickSlots.FirstSlot.performed += i => first_slot = true;
        inputActions.QuickSlots.SecondSlot.performed += i => second_slot = true;
        inputActions.QuickSlots.ThirdSlot.performed += i => third_slot = true;
        inputActions.QuickSlots.FourthSlot.performed += i => fourth_slot = true;
        
        // TODO switch

        if (playerManager.isInteracting == false)
        {
            if (first_slot)
            {
                inventory.ChangeRightWeapon();
                Debug.Log("1");
            }
            else if (second_slot)
            {
                inventory.ChangeLeftWeapon();
                Debug.Log("2");
            }
            // else if (third_slot)
            // {
            //     
            // }
            // else if (fourth_slot)
            // {
            //     
            // }
        }
        
    }

    public void HandleInteractableButtonInput()
    {
        inputActions.PlayerActions.Interaction.performed += i => interactable_Input = true;
    }

    public void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
    }

    public void HandleMenuInput()
    {
        inputActions.PlayerActions.Menu.performed += i => menu_Input = true;
        
        if (menu_Input)
        {
            menuFlag = !menuFlag;

            if (menuFlag)
            {
                UIManager.instance.OpenMenu();
            }
            else
            {
                UIManager.instance.CloseMenu();
            }
        }
    }
    
    public void HandleInventoryInput()
    {
        inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                UIManager.instance.OpenInventory();
                InventoryUI inventoryUI = UIManager.instance.Menu.inventoryUI;
                inventoryUI.UpdateUI();
            }
            else
            {
                UIManager.instance.CloseInventory();
            }
        }
    }
}
