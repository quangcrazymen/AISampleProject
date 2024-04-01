using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float speed = 12f;

    public float gravity = -9.81f;
    Vector3 velocity;

    public Transform Feet;
    public float jumpingHeight = 400.0f;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.4f, LayerMask.GetMask("Ground"));
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(Feet.position, 0.4f, LayerMask.GetMask("Ground"));
        if(isGrounded && velocity.y<0.0f)
        {   
            velocity.y = -1.0f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            velocity.y = Mathf.Sqrt(jumpingHeight - 9.81f * 9.81f);
        }
        Vector3 move = transform.right * x + transform.forward * z;
        Controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }
}
