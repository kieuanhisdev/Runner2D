using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move infor")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private bool playerUnlocked;
    private bool canDoubleJump;
    private float defaultJumpPorce;


    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defaultJumpPorce = jumpForce;
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorController();

        if (playerUnlocked)
        {

            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }


        checkCollision();

        checkInput();
    }

    private void AnimatorController()
    {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xValocity", rb.linearVelocityX);
        anim.SetFloat("yValocity", rb.linearVelocityY);
    }

    private void checkCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void checkInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            playerUnlocked = !playerUnlocked;
        }


        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();

        }
    }

    private void JumpButton()
    {

        if (isGrounded)
        {
            canDoubleJump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        }
        else if (canDoubleJump)
        {
            jumpForce = doubleJumpForce;
            canDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            jumpForce = defaultJumpPorce;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
