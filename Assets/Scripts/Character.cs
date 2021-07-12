using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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

    [SerializeField]
    private float m_climbingSpeed = 5.0f;

    // Jump force
    [SerializeField] 
    private float m_jumpForce = 10.0f;

    // Ground Check
    private bool m_isGrounded;

    // Touching other player check
    private bool m_touchingOtherPlayer;

    // Other player
    [SerializeField]
    private Character m_otherPlayer;

    // Bond between the players
    [SerializeField]
    private CharacterBond m_bond;

    // X input (for moving character left and right)
    [SerializeField]
    private float m_xInput;

    // Y direction (for moving character up and down when character is off the ground)
    [SerializeField]
    private float m_yInput;

    // Player animations
    [SerializeField]
    private Animator m_playerAnimator;

    private Vector2 m_playerPos;

    // Start is called before the first frame update
    private void Awake()
    {
        // Check the value held by the game objects tag
        if(tag == "Player_1")
        {
            // "Player_1" tag found, set the player index to 0
            m_playerIndex = 0;
        }
        else if (tag == "Player_2")
        {
            // "Player_2" tag found, set the player index to 1
            m_playerIndex = 1;
        }
        
        // Obtain the 2D rigid body component
        m_rb = GetComponent<Rigidbody2D>();

        // Obtain bond component
        m_bond = GameObject.FindGameObjectWithTag("Bond").GetComponent<CharacterBond>();

        // Obtain the player animator component
        m_playerAnimator = GetComponent<Animator>();

        // Obtain reference to the other player
        if (m_playerIndex == 0)
        {
            // Current player is player 1, obtain player 2
            m_otherPlayer = GameObject.FindGameObjectWithTag("Player_2").GetComponent<Character>();
        }
        else if (m_playerIndex == 1)
        {
            // Current player is player 2, obtain player 1
            m_otherPlayer = GameObject.FindGameObjectWithTag("Player_1").GetComponent<Character>();
        }
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
        // 
        //m_rb.velocity = new Vector2(m_xInput * m_movementSpeed, m_rb.velocity.y);

        // Create a vector2 based on the x value, movement speed and keep y value the same
        Vector2 newPos = Vector2.zero;
        // Check that the input does not exceed the y distance
        // If fine, update the rigid body
        // If not, do not update the rigid body

        // Determine the new position, based on the new values of x and y
        newPos = new Vector2(transform.position.x, transform.position.y) + (new Vector2(m_xInput, 0.0f) * Time.deltaTime * m_movementSpeed);
        //newPos = new Vector2(transform.position.x, transform.position.y) + new Vector2(m_xInput, 0.0f);

        if (m_bond.AllowableDistance(newPos, m_otherPlayer.transform.position) == false)
        {
            // Determine the distance between the new player position and the other player
            Vector2 vectorBetweenPlayers = (newPos - m_otherPlayer.GetPlayerPosition());

            // Determine the new player position that will not violate the maximum bond length
            newPos = m_otherPlayer.GetPlayerPosition() + (vectorBetweenPlayers.normalized * m_bond.ReturnMaxLength());
        }

        // Check if the player is not grounded
        if (!m_isGrounded)
        {
            // Check if the player is below the other player
            if (GetPlayerPosition().y < m_otherPlayer.GetPlayerPosition().y && m_bond.AllowableDistance(newPos, m_otherPlayer.transform.position) == false)
            {
                // Set the vertical velocity to 0 (Cheap hack to get the vertical velocity)
                m_rb.velocity = new Vector2(m_rb.velocity.x, 0.0f);
            }
        }
        // Update the player position to the new position
        m_playerPos = newPos;

        // Update the transform, based on the value held by the player position vector
        transform.position = m_playerPos;

        // CHARACTER ANIMATIONS
        // Flip the sprite based on the value of the X Input variable
        if (m_xInput < 0)
        {
            // Input is less than zero, do not flip sprite (facing to the left by default)
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (m_xInput > 0)
        {
            // Input
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // Check if the X direction is non-zero
        if (m_xInput != 0 && m_isGrounded)
        {
            // Enable the walking animation
            m_playerAnimator.SetBool("isWalking", true);
        }
        else if (m_xInput == 0 && m_isGrounded)
        {
            // Disable the walking animation
            m_playerAnimator.SetBool("isWalking", false);
        }

        // Animate jumping and falling based on the direction of the movement
        if (!m_isGrounded && !m_touchingOtherPlayer)
        {
            // Determine the player's movement direction
            Vector3 direction = transform.InverseTransformDirection(m_rb.velocity); //transform.position - previousPos;

            // Check Y value of the direction vector
            if (direction.y < 0)
            {
                // Y value is less than 0, player is falling

                // Enable falling animation
                m_playerAnimator.SetBool("isFalling", true);

                // Disable jumping animation
                m_playerAnimator.SetBool("isJumping", false);
            }
            else if (direction.y > 0)
            {
                // Y value is greater than 0, player is jumping

                // Disable falling animation
                m_playerAnimator.SetBool("isFalling", false);

                // Enable jumping animation
                m_playerAnimator.SetBool("isJumping", true);
            }
        }
        else
        {
            // Disable falling animation
            m_playerAnimator.SetBool("isFalling", false);

            // Disable jumping animation
            m_playerAnimator.SetBool("isJumping", false);
        }
    }

    /*
     * MOVE METHOD
     * 
     * Method for handling movement of the player
     */
    public void SetXValue(float xValue)
    {
        // Set the value of the X direction variable based on the value from the argument
        m_xInput = xValue;
    }

    public void SetYValue(float yValue)
    {
        // Set the value of the Y direction variable based on the value from the argument
        m_yInput = yValue;
    }

    /*
     * JUMP METHOD
     * 
     * Method for handling the jump action
     */
    public void Jump()
    {
        // Check if the player is grounded or it touching the other player (need improvement)
        if (m_isGrounded || m_touchingOtherPlayer)
        {
            // Invoke the jump action in Y, whilst keeping the rigid body velocity in X
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);

            // Enable jumping animation
            m_playerAnimator.SetBool("isJumping", true);
        }
    }

    /*
     * GRAB METHOD
     * 
     * Method for handling the player when
     * they are grabbing
     */
    public void Grab(bool boolValue)
    {
        // Add grabbing code here
    }

    /*
     * GET PLAYER INDEX METHOD
     * 
     * Method returns the index number of
     * the player.
     * 
     * 0 = Player 1
     * 1 = Player 2
     */
    public int GetPlayerIndex()
    {
        // Return the value held by the player index variable
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

        // Check if the player is touching other player (11 is the environment layer)
        if (collision.gameObject.layer == 10 && m_otherPlayer.CheckGrounded())
        {
            // Set touching other player to true
            m_touchingOtherPlayer = true;
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
            // Set is grounded to false
            m_isGrounded = false;
        }

        // Check if the player is touching other player (10 is the environment layer)
        if (collision.gameObject.layer == 10 && m_otherPlayer.CheckGrounded())
        {
            // Set is grounded to true
            m_touchingOtherPlayer = false;
        }
    }

    /*
     * CHECK GROUNDED METHOD
     * 
     * Method returns the value held
     * by the isGrounded variable.
     */
    public bool CheckGrounded()
    {
        // Return the value of is grounded
        return m_isGrounded;
    }

    /*
     * CHECK TOUCHING PLAYER METHOD
     * 
     * Method returns the value held
     * by the touchingOtherPlayer
     * variable.
     */
    public bool CheckTouchingPlayer()
    {
        // Return the value held by touching other player
        return m_touchingOtherPlayer;
    }

    public Vector2 GetPlayerPosition()
    {
        return m_playerPos;
    }
}
