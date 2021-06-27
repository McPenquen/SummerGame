using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Player Index Number
    [SerializeField]
    private int m_playerIndex = 0;

    // Player Rigid Body
    [SerializeField] 
    private Rigidbody2D m_rb;

    // Movement Speed
    [SerializeField] 
    private float m_movementSpeed = 5.0f;

    // Jump force
    [SerializeField] 
    private float m_jumpForce = 10.0f;

    // Ground Check
    private bool m_isGrounded;

    // X direction
    private float m_xDir;

    // Start is called before the first frame update
    private void Awake()
    {
        // Obtain the 2D rigid body attached to the game object
        m_rb = GetComponent<Rigidbody2D>();
    }

    /*
     * UPDATE METHOD
     * 
     * Method is invoked once per frame.
     * 
     * Method updates the velocity of the 
     * 2D rigid body attached to the 
     * character.
     */
    void Update()
    {
        // Alter the velocity of the player based on the value of the X direction float
        m_rb.velocity = new Vector2(m_xDir * m_movementSpeed, m_rb.velocity.y);
    }

    /*
     * MOVE METHOD
     * 
     * Method for handling movement of the player
     */
    public void SetXValue(float xValue)
    {
        // Read the value of the x component of the analogue stick
        m_xDir = xValue;
    }

    /*
     * JUMP METHOD
     * 
     * Method for handling the jump action
     */
    public void Jump()
    {
        // Check if the player is grounded
        if (m_isGrounded)
        {
            // Invoke the jump action in Y, whilst keeping the rigid body velocity in X
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
        }
    }

    public int GetPlayerIndex()
    {
        return m_playerIndex;
    }

    /*
     * ON COLLISION ENTER 2D METHOD
     * 
     * Standard Unity method for handling
     * entries to 2D collisions.
     * 
     * Method first checks that the collision exit
     * has occured with the environment layer.
     * If so, it sets the "is Grounded" Boolean
     * to true.
     * 
     * collisiond handling inspired from: https://answers.unity.com/questions/1220752/how-to-detect-if-not-colliding.html
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is touching the ground (11 is the environment layer)
        if (collision.gameObject.layer == 11)
        {
            // Set is grounded to true
            m_isGrounded = true;
        }
    }

    /*
     * ON COLLISION EXIT 2D METHOD
     * 
     * Standard Unity method for handling
     * exits from 2D collisions.
     * 
     * Method first checks that the collision exit
     * has occured with the environment layer.
     * If so, it sets the "is Grounded" Boolean
     * to false.
     */
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check that the collision exit has occured with the ground layer (11 is the environment layer)
        if (collision.gameObject.layer == 11)
        {
            m_isGrounded = false;
        }
    }
}
