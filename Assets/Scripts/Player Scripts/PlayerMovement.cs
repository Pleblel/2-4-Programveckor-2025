using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{

    //Darren

    [Header("PlayerDirections")]
    float diretionX;
    float directionZ;
    Vector3 movementDirection;

    [Header("References")]
    Rigidbody rb;

    [Header("PlayerVariables")]
    [SerializeField] float jumpPower = 5.0f;
    [SerializeField] float dashPower = 5.0f;
    [SerializeField] float dashTime = 0.3f;
    [SerializeField] float dashCoolDown = 1.0f;
    bool inAir = false;
    bool canDash = true;
    bool isDashing = false;



    public float movementSpeed { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movementSpeed = 6.0f;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get the movement direction inputs
        movementDirection = new Vector3(diretionX, 0f, directionZ).normalized;
        diretionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");

        //Returns the line of code, not letting anything under it run
        if (isDashing)
            return;

        DashInput();
        Jump();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;
        Move(movementDirection);
    }


    public void Move(Vector3 direction)
    {
        //Get the direction, multiply the directions by movement speed, store the target velocity on the "y" axis to ensure no gravity issues
        Vector3 targetVelocity = direction * movementSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }


    private void DashInput()
    {
        //"Fire3" is the left shift keybind
        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine("Dash");
        }
    }


    private IEnumerator Dash()
    {
        //Dashes the player forward (z-axis)
        canDash = false;
        isDashing = true;
        Vector3 dashDiretion = transform.forward;
        rb.velocity = dashDiretion * dashPower;
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector3.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }




    private void Jump()
    {
        //Use the absolute power of rb.velocity.y to check if the player is in the air or not
        inAir = Mathf.Abs(rb.velocity.y) > 0.01f;

        if (Input.GetButton("Jump") && !inAir)
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }
}
