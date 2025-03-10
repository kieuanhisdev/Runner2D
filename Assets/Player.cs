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

    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCoolDown;
    [SerializeField] private float cellingCheckDistance;
    private float slideCoolDownCouter;
    private float slideTimeCouter;
    private bool isSliding;
    private bool cellingDistance;

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

        if (playerUnlocked)
        {
            Movenment();
        }
        if (isGrounded) canDoubleJump = true;

        slideTimeCouter -= Time.deltaTime;
        slideCoolDownCouter -= Time.deltaTime;

        checkCollision();
        checkForSlide();

        checkInput();
    }

    private void checkForSlide()
    {
        if (slideTimeCouter < 0  && !cellingDistance)
        {
            isSliding = false;
        }
    }

    private void Movenment()
    {
        if (wallDetected) return;

        if (isSliding)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            slideButton();
        }
    }

    private void slideButton()
    {
        if (rb.linearVelocityX != 0 && slideCoolDownCouter < 0)
        {
            isSliding = true;
            slideTimeCouter = slideTime;
            slideCoolDownCouter = slideCoolDown;
        }

    }

    private void JumpButton()
    {
        if (isSliding) return;

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

    private void AnimatorController()
    {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetFloat("xValocity", rb.linearVelocityX);
        anim.SetFloat("yValocity", rb.linearVelocityY);
    }
    private void checkCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        cellingDistance = Physics2D.Raycast(transform.position, Vector2.up, cellingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + cellingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
