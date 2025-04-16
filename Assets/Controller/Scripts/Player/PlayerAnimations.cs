using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PlayerController;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    public Animator animator;
    private IPlayerController player;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        player = GetComponentInParent<IPlayerController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Math.Abs(player.Velocity.x));
        animator.SetFloat("yVelocity", player.Velocity.y);
        animator.SetBool("Grounded", player.State.Grounded);
        player.GroundedChanged += OnGroundedChanged;
        player.Jumped += OnJumped;
        player.WallGrabChanged += OnWallGrabChanged;        
    }
    private void OnJumped(JumpType type)
    {
        if (type is JumpType.Jump or JumpType.Coyote)
        {
            animator.SetBool("Jump", true);
        }
        else if (type is JumpType.AirJump)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("AirJump", true);
        }
        else if (type is JumpType.WallJump)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("WallJump", true);
            Debug.Log("WallJump");
        }
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        if (grounded)
        {
            animator.SetBool("Grounded", true);
            animator.SetBool("Jump", false);
            animator.SetBool("WallJump", false);
            animator.SetBool("AirJump", false);
        }else if (!grounded){
            animator.SetBool("Grounded", false);
        }
    }


    private void OnWallGrabChanged(bool onWall)
    {
        animator.SetBool("OnWall", onWall);
        animator.SetBool("Jump", !onWall);
        animator.SetBool("WallJump", !onWall);
        animator.SetBool("AirJump", !onWall);
    }
}
