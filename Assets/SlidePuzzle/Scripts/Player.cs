using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject mainCamera;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity; // to hold temp value for angle smoothing
    
    private Vector3 verticalVelocity;
    private Vector3 playerKeyboardInput;
    
    private Animator animator;
    private float animationBlend;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        //normalise so that it does not get too fast when moving diagonally
        //basically normalise the vector to a direction, or use .normalize
        //Vector3 direction = transform.TransformDirection(playerKeyboardInput);
        
        //playerKeyboardInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        playerKeyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 direction = playerKeyboardInput.normalized;
        
        //proceed only if there is input
        if (direction.magnitude < 0.1f)
        {
            animator.SetFloat(Animator.StringToHash("speed"), 0);
            return;
        }

        if (controller.isGrounded)
        {
            verticalVelocity.y = -1f;
            //only jump when grounded
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity.y = jumpForce;
            }
        }
        else
        {
            //standard physics vertical velocity formula 
            verticalVelocity.y -= gravity * 2f * Time.deltaTime;
        }

        direction = turnTo(direction, true);
        controller.Move(direction * speed * Time.deltaTime);
        controller.Move(verticalVelocity * Time.deltaTime);
        
        //lerp to the full speed for nice blending, but doesn't seem to work well for me
        animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime);
        animator.SetFloat(Animator.StringToHash("speed"), animationBlend);
    }

    private Vector3 turnTo(Vector3 direction, bool isSmooth)
    {
        //use atan x/y because we are facing positive y, standard atan y/x starts the angle 0deg from positive x if drawn on cartesian
        //atan is in rad, and convert to deg by multiplying
        //use z instead of y because we don't move up
        float turnTo = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //make character go forward to the angle where main camera is facing.
        turnTo += mainCamera.transform.eulerAngles.y;

        if (isSmooth)
        {
            //smooth the angle to make it anime nicely
            turnTo =
                Mathf.SmoothDampAngle(transform.eulerAngles.y, turnTo, ref turnSmoothVelocity, turnSmoothTime);

        }
        //turn object
        transform.rotation = Quaternion.Euler(0, turnTo, 0);
        
        //to move, now we need to convert the angle to a direction
        direction = Quaternion.Euler(0, turnTo, 0) * Vector3.forward;
        return direction.normalized;
    }
}
