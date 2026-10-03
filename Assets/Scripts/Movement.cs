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
    public GameObject Blink;
    public GameObject proceedtext;
    public GameObject PROCEEDTEXT;

    public bool DoubleJumpUnlocked;
    public bool DashUnlocked;
    public bool SlamUnlocked;
    public GameObject Refusaltext;
    // Start is called before the first frame update
    void Start()
    {
        MyVelocity = 0;
        MyRb = GetComponent<Rigidbody2D>();
        CanMove = true;
        DoubleJumpUnlocked = true;
        DashUnlocked = true;
        SlamUnlocked = true;
        movespeed = 7; //10
        dashforce = 22; //22

        inputX = 0;

        
}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift) && CanMove == true)
        {
            movespeed = 3.5f;
            dashforce = 16;
        }
        else
        {
            movespeed = 7;
            dashforce = 22;
        }


        if (IsDashing == false && IsDiving == false)
        {
            
            MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }

        if (IsDiving == true)
        {
            CanMove = false;
        }


            if (Input.GetKey(KeyCode.D) && CanMove == true && IsDashing == false)
            {
            inputX = 1;
                MyRb.position += Vector2.right * inputX * movespeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A) && CanMove == true && IsDashing == false)
            {
            inputX = -1;
                MyRb.position += Vector2.right * movespeed * inputX * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == true && IsDiving == false)
            {
            MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            MyRb.AddForce(Vector2.up * 18f, ForceMode2D.Impulse);
            Invoke("givedoublejump", 0.15f);
            }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == false && IsDiving == false && jumpedonce == true && DoubleJumpUnlocked == true)
        {
            MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
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
            //jumpedonce = true;
            }

        if (Input.GetKeyDown(KeyCode.E) && CanMove == true && DashOnCD == false && IsDashing == false && IsGrounded == false && dashedonce == false && DashUnlocked == true)
        {
            MyRb.velocity = Vector3.zero;
            CanMove = false;
            IsDashing = true;
            MyRb.AddForce(Vector2.right * dashforce * Mathf.RoundToInt(inputX), ForceMode2D.Impulse);
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            DashOnCD = true;
            Invoke("GiveMovement", 0.5f);
            Invoke("GiveDash", 0.5f);
            dashedonce = true;
            

        }

        if (Input.GetKeyDown(KeyCode.S) && SlamOnCD == false && IsDiving == false && SlamUnlocked == true)
            {
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            IsDiving = true;
            IsDashing = false;
            CanMove = false;
            SlamOnCD = true;
            Invoke("UnfreezeY", 0.15f);
            Invoke("Slam", 0.15f);  
            Invoke("GiveSlam", 0.2f);
            }

        
    }

    public void UnfreezeY()
    {
        MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }
    public void Blinking()
    {
        Blink.SetActive(true);
        Invoke("Unblink", 0.3f);
    }

    public void Unblink()
    {
        Blink.SetActive(false);
    }
    public void givedoublejump()
    {
        jumpedonce = true;
    }
    public void Slam()
    {
        MyRb.gravityScale = 500f;
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
        MyRb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
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
            Blinking();
            transform.position = new Vector2(-11f, -2.882037f);
            Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
            MyRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            //SlamUnlocked = false;
            //DashUnlocked = false;
            //DoubleJumpUnlocked = false;
            //movespeed = 7;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IsGrounded = true;
        dashedonce = false;
        jumpedonce = false;

        if (Input.GetKeyDown(KeyCode.W) && collision.gameObject.CompareTag("DiveDoor"))
        {
            Blinking();
            transform.position = new Vector2(33.9f, -50);
            Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
            SlamUnlocked = true;
        }

        if (Input.GetKeyDown(KeyCode.W) && collision.gameObject.CompareTag("DashDoor"))
        {
            Blinking();
            Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
            SlamUnlocked = true;
            collision.gameObject.SetActive(false);
            Refusaltext.SetActive(false);
            PROCEEDTEXT.SetActive(true);
            proceedtext.SetActive(false);
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


