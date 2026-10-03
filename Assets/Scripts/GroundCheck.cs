using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class GroundCheck : MonoBehaviour
{
    public CapsuleCollider2D MyBc;
    public Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D Collision)
    {
        if (Collision.gameObject.CompareTag("Ground"))
        {
            movement.IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D Collision)
    {
        movement.IsGrounded = false;
    }
}
