using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator animator;
    
    private PlayerManager playerManager;
    private InputHandler inputHandler;
    private PlayerLocomotion playerLocomotion;
    
    private int ver;
    private int hor;
    public float v = 0;
    public bool canRotate;

    public void Initialize()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();

        ver = Animator.StringToHash("Vertical");
        hor = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verMovement, float horMovement, bool isSprinting, bool isWalking)
    {
        #region Vertical

        if(verMovement > 0 && verMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if(verMovement > 0.55f)
        {
            v = 1;
        }
        else if(verMovement < 0 && verMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if(verMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion
        
        #region Horizontal

        float h = 0;

         if(horMovement > 0 && horMovement < 0.55f)
         {
             h = 0.5f;
         }
         else if(horMovement > 0.55f)
         {
             h = 1;
         }
         else if(horMovement < 0 && horMovement > -0.55f)
         {
             h = -0.5f;
         }
         else if(horMovement < -0.55f)
         {
             h = -1;
         }
         else
         {
             h = 0;
         }
         #endregion

         if (isSprinting)
         {
             v = 2;
             h = horMovement;
         }
         
         if (isWalking)
         {
             v = 0.5f;
             h = horMovement;
         }
         
        animator.SetFloat(ver, v, 0.1f, Time.deltaTime);
        animator.SetFloat(hor, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        animator.SetBool("CanDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("CanDoCombo", false);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    private void OnAnimatorMove()
    {
        // if (playerManager.isInteracting != true)
        // {
        //     return;
        // }
        
        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }
}
