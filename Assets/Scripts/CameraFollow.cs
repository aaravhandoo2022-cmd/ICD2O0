using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public float speed;
    public Vector2 v2;
    public Camera Cam;
    // Start is called before the first frame update
    void Start()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetposition = new Vector3(Player.transform.position.x + v2.x, Player.transform.position.y + v2.y, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, targetposition, speed * Time.deltaTime);
       
    }
}
