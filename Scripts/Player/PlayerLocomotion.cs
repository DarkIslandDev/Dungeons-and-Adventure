using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Transform cameraObject;
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    
    public Vector3 moveDirection;

    [HideInInspector] public Transform myTransform;

    [HideInInspector] public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Статы обнаружения земли и воздуха")] 
    [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] private float minDistanceNeededToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.2f;
    private LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [Header("Статы")] 
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float sprintSpeed = 20;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float fallingSpeed = 80;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    

    #region Movement

    private Vector3 normalVector;
    private Vector3 targetPosition;

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
        {
            return;
        }

        if (playerManager.isInteracting)
        {
            return;
        }
        
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }

        Vector3 projectiveVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectiveVelocity;
        
        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount,
            0, playerManager.isSprinting, playerManager.isWalking);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.animator.GetBool("isInteracting"))
        {
            return;
        }

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f && inAirTimer < 0.55f)
                {
                    // Debug.Log("In air " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("LandToGround", true);
                    // rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    // rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                    inAirTimer = 0;
                }
                else if (inAirTimer > 0.55f)
                {
                    animatorHandler.PlayTargetAnimation("LandToRoll", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    // rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    // rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                    rigidbody.constraints = RigidbodyConstraints.None;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }
                else
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.isGrounded || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting == false)
        {
            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    animatorHandler.canRotate = false;
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                    Debug.Log("прыгнуто");
                }
            }

            animatorHandler.canRotate = true;
        }
    }
    
    #endregion
}
