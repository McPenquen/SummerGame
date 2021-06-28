using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // Player Rigid Body
    [SerializeField] private Rigidbody2D m_rb;

    // Movement Speed
    [SerializeField] private float m_movementSpeed = 5.0f;

    // Jump force
    [SerializeField] private float m_jumpForce = 10.0f;

    // Ground Check
    private bool isGrounded;

    // X Input
    private float inputX;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Alter the velocity of the player
        m_rb.velocity = new Vector2(inputX * m_movementSpeed, m_rb.velocity.y);

        //// Check if velocity of the rigid body is greater than 0
        //if(m_rb.velocity.x > 0.0f)
        //{
        //    transform.localScale = Vector3.one;
        //}
        //else if(m_rb.velocity.x <0.0f)
        //{
        //    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        //}
    }

    /*
     * MOVE METHOD
     * 
     * Method for handling movement of the player
     */
    public void Move(InputAction.CallbackContext context)
    {
        // Read the value of the x component of the analogue stick
        inputX = context.ReadValue<Vector2>().x;
    }

    /*
     * JUMP METHOD
     * 
     * Method for handling the jump action
     */
    public void Jump(InputAction.CallbackContext context)
    {
        // Check if the context has just been performed and the player is grounded
        if(context.performed && isGrounded)
        {
            // Invoke the jump action in Y, whilst keeping the rigid body velocity in X
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
        }
    }

    // collisiond handling inspired from: https://answers.unity.com/questions/1220752/how-to-detect-if-not-colliding.html
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is touching the ground (11 is the environment layer)
        if (collision.gameObject.layer == 11)
        {
            // Set is grounded to true
            isGrounded = true;
        }

    }
    
    // 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            isGrounded = false;
        }
    }
}
