using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Movement : MonoBehaviour
{
    public SpriteRenderer MySprite;
    public GameObject Player;
    public Rigidbody2D MyRb;
    public bool IsGrounded;
    public float movespeed;
    public float dashforce;
    public float inputX;
    public bool CanMove;
    public bool SlamOnCD;
    public bool DashOnCD;
    public bool IsDashing;
    public bool IsDiving;
    public BoxCollider2D MyBc;
    public float MyVelocity;
    public bool dashedonce;
    public bool jumpedonce;
    public Camera Cam;

    public bool DoubleJumpUnlocked;
    public bool DashUnlocked;
    public bool SlamUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        MyVelocity = 0;
        MyRb = GetComponent<Rigidbody2D>();
        CanMove = true;
        DoubleJumpUnlocked = false;
        DashUnlocked = false;
        SlamUnlocked = false;
        movespeed = 7; //10
        dashforce = 22; //22

        inputX = 0;
}

    // Update is called once per frame
    void Update()
    {


        if (IsDashing == false)
        {
            
            MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }


            if (Input.GetKey(KeyCode.D) && CanMove == true)
            {
            inputX = 1;
                MyRb.position += Vector2.right * inputX * movespeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A) && CanMove == true)
            {
            inputX = -1;
                MyRb.position += Vector2.right * movespeed * inputX * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == true && CanMove == true)
            {
                MyRb.AddForce(Vector2.up * 18f, ForceMode2D.Impulse);
            Invoke("givedoublejump", 0.15f);
            }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == false && CanMove == true && jumpedonce == true && DoubleJumpUnlocked == true)
        {
            MyRb.AddForce(Vector2.up * 18f, ForceMode2D.Impulse);
            jumpedonce = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && CanMove == true && DashOnCD == false && IsDashing == false && IsGrounded == true && DashUnlocked == true)
            {
                MyRb.velocity = Vector3.zero;
                CanMove = false;
                IsDashing = true;
                MyRb.AddForce(Vector2.right * dashforce * Mathf.RoundToInt(inputX), ForceMode2D.Impulse);
                MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
                DashOnCD = true;
                Invoke("GiveMovement", 0.50f);
                Invoke("GiveDash", 0.50f);
            }

        if (Input.GetKeyDown(KeyCode.E) && CanMove == true && DashOnCD == false && IsDashing == false && IsGrounded == false && dashedonce == false && DashUnlocked == true)
        {
            MyRb.velocity = Vector3.zero;
            CanMove = false;
            IsDashing = true;
            MyRb.AddForce(Vector2.right * dashforce * Mathf.RoundToInt(inputX), ForceMode2D.Impulse);
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            DashOnCD = true;
            Invoke("GiveMovement", 0.50f);
            Invoke("GiveDash", 0.50f);
            dashedonce = true;
            jumpedonce = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && SlamOnCD == false && IsDiving == false && SlamUnlocked == true)
            {
                Slam();
                IsDiving = true;
                IsDashing = false;
                CanMove = false;
                SlamOnCD = true;
                MyRb.velocity = Vector2.zero;
                Invoke("GiveSlam", 0.2f);
            }

        
    }

    public void givedoublejump()
    {
        jumpedonce = true;
    }
    public void Slam()
    {
        MyRb.gravityScale = 31f;
    }
    public void GiveMovement()
    {
        CanMove = true;
    }

    public void GiveDash()
    {
        DashOnCD = false;
        IsDashing = false;
    }

    public void GiveSlam()
    {
        SlamOnCD = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsDiving == true)
        {
            IsGrounded = true;
        }
        {
           // IsGrounded = true;
        }
       // IsGrounded = true;
        IsDiving = false;
        MyRb.gravityScale = 2.5f;
        Invoke("GiveMovement", 0.2f);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       // IsGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = new Vector2(-11f, -2.882037f);
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            SlamUnlocked = false;
            DashUnlocked = false;
            DoubleJumpUnlocked = false;
            movespeed = 6;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IsGrounded = true;
        dashedonce = false;
        jumpedonce = false;

        if (Input.GetKeyDown(KeyCode.W) && collision.gameObject.CompareTag("DiveDoor"))
        {
            transform.position = new Vector2(33.9f, -50);
            SlamUnlocked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsGrounded = false;
    } 

}


