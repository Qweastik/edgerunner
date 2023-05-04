using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 moveVector;
    public float speed = 2f;
    public Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        WallcheckRadiusDown = WallCheckDown.GetComponent<CircleCollider2D>().radius;
        gravityDef = rb.gravityScale;
    }


    void Update()
    {
        walk();
        jump();
        Dash();
        Reflect();
        MoveOnWall();
        WallJump();

    }

    private void FixedUpdate()
    {
        CheckingGround();
        CheckingWall();
        CheckingLege();
    }


    public bool faceRight = true;
    void walk()
    {
        if (!blockMoveX)
        {
            moveVector.x = Input.GetAxis("Horizontal");
            anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
            rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
        }

    }

    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }




    public float jumpForce = 5f;
    private bool jumpControl;
    public int jumpIteration = 0;
    private int jumpValueIteration = 60;

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            Invoke("IgnoreLayerOff", 0.5f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (onGround) { jumpControl = true; }
        }
        else { jumpControl = false; }

        if (jumpControl)
        {
            if (jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
            }
        }
        else { jumpIteration = 0; }
    }

    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius = 0.5f;
    public LayerMask Ground;

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }


    public int dashImpulse = 5000;
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.C) && !lockDash)
        {
            lockDash = true;
            Invoke("DashLock", 0.7f);
            anim.StopPlayback();
            anim.Play("Dash");

            rb.velocity = new Vector2(0, 0);

            if (!faceRight) { rb.AddForce(Vector2.left * dashImpulse); }
            else { rb.AddForce(Vector2.right * dashImpulse); }
        }
    }

    public bool lockDash = false;

    void DashLock()
    {
        lockDash = false;
    }



    public bool onWall;
    public bool onWallUp;
    public bool onWallDown;
    public LayerMask Wall;
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public float WallcheckRayDistance = 1f;
    private float WallcheckRadiusDown;
    public bool onLedge;
    public float ledgeRayCorrectY = 0.5f;
    

    void CheckingWall()
    {
        onWallUp = Physics2D.Raycast(WallCheckUp.position, new Vector2(transform.localScale.x, 0), WallcheckRayDistance, Wall);
        onWallDown = Physics2D.OverlapCircle(WallCheckDown.position, WallcheckRadiusDown, Wall);
        onWall = (onWallUp && onWallDown);
        anim.SetBool("onWall", onWall); 
        
    }

    void CheckingLege()
    {
        if (onWallUp)
        {
            onLedge = !Physics2D.Raycast
            (
                new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY),
                new Vector2(transform.localScale.x, 0),
                WallcheckRayDistance,
                Wall
            );
        }
        else { onLedge = false; }

        if (onLedge)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            offsetCalculateAndCorrect();
        }
    }
    public float minCorrectDistance = 0.01f;
    public float offsetY;
    void offsetCalculateAndCorrect()
    {
        offsetY = Physics2D.Raycast
            (
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRayCorrectY),
                Vector2.down,
                ledgeRayCorrectY,
                Ground
            ).distance;

        if(offsetY > minCorrectDistance * 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - offsetY + minCorrectDistance, transform.position.z);
        }
    }



    public float upDownSpeed = 4f;
    public float slideSpeed = 0;
    public float gravityDef;
    void MoveOnWall()
    {
        if (onWall && !onGround)
        {
            moveVector.y = Input.GetAxisRaw("Vertical");
            anim.SetFloat("UpDown", moveVector.y);

            if (!blockMoveX)
            {
                if (moveVector.y == 0)
                {
                    rb.gravityScale = 0;
                    rb.velocity = new Vector2(0, slideSpeed);
                }
                
            }

            
            if (moveVector.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveVector.y * upDownSpeed / 2);
            }
            else if (moveVector.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveVector.y * upDownSpeed);
            }
        }

        else if (!onGround && !onWall) { rb.gravityScale = gravityDef; }
    }


    private bool blockMoveX;
    public float jumpWallTime = 0.3f;
    private float timerJumpWall;
    public Vector2 jumpAngle = new Vector2(3.5f, 10);
    void WallJump()
    {
        if(onWall && !onGround && Input.GetKeyDown(KeyCode.Space))
        {
            blockMoveX = true;

            moveVector.x = 0;

            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;

            rb.gravityScale = gravityDef;
            rb.velocity = new Vector2(0, 0);

            rb.velocity = new Vector2(transform.localScale.x * jumpAngle.x, jumpAngle.y);
        }

        if (blockMoveX && (timerJumpWall +=Time.deltaTime) >= jumpWallTime)
        {
            if(onWall || onGround || Input.GetAxisRaw("Horizontal") != 0)
            {
                blockMoveX = false;
                timerJumpWall = 0;
            }
            
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(WallCheckUp.position, new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y));
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine
            (
                new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY),
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRayCorrectY)
            );

        Gizmos.color = Color.green;
        Gizmos.DrawLine
            (
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRayCorrectY),
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y)
            );
    }
}
