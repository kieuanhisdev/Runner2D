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



    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool wallDetected;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorController();

        if (playerUnlocked && !wallDetected)
        {

            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }
        if(isGrounded) canDoubleJump = true;


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
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
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
            
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
