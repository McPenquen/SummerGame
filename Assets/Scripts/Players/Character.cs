using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Character : MonoBehaviour
{
    // PLAYER VALUES
    [Header("Player Values")]

    // Player Index Number
    [SerializeField]
    private int m_playerIndex = 0;

    // Player position
    [SerializeField]
    private Vector2 m_playerPos;

    [SerializeField]
    private Vector2 m_lastPlayerPos;

    // PLAYER COMPONENTS
    [Header("Player Components")]

    // Player Rigid Body
    [SerializeField] 
    private Rigidbody2D m_rb;

    // Player animations
    [SerializeField]
    private Animator m_playerAnimator;

    // CONTROLL VARIABLES
    [Header ("Control Variables")]
    
    // Movement Speed
    [SerializeField]
    private float m_movementSpeed = 5.0f;

    // Repelling speed
    [SerializeField]
    private float m_climbingSpeed = 5.0f;

    // Jump force
    [SerializeField] 
    private float m_jumpForce = 10.0f;

    // PLAYER STATES
    [Header("Player States")]

    // Is Touching other player
    [SerializeField]
    private bool m_touchingOtherPlayer;

    // Is Grounded
    [SerializeField]
    private bool m_isGrounded;

    // Is Jumping
    [SerializeField]
    private bool m_isJumping;

    // Is Falling
    [SerializeField]
    private bool m_isFalling;

    // Is swinging
    [SerializeField]
    private bool m_isSwinging;

    // Is gripping
    [SerializeField]
    private bool m_isGrabbing;

    // Can grab
    [SerializeField]
    private bool m_canGrab;

    // Can move
    [SerializeField]
    private bool m_canMove;

    // Grab radius
    [SerializeField]
    private float m_grabRadius = 1.5f;

    // Layer Mask   
    [SerializeField]
    private LayerMask m_grabbableLayer = 11;

    // OBJECT REFERENCES
    [Header("Object References")]
    // Other player
    [SerializeField]
    private Character m_otherPlayer;

    // Bond between the players
    [SerializeField]
    private CharacterBond m_bond;

    // CONTROLLER INPUT VALUES
    [Header("Controller Input Values")]

    // X input (for moving character left and right)
    [SerializeField]
    private float m_xInput;

    // Y direction (for moving character up and down when character is off the ground)
    [SerializeField]
    private float m_yInput;

    // DEBUG GRAB CIRCLE
    [Header("Debug Grab Circle")]

    // View Grab Circle boolean - DEBUG USE ONLY
    [SerializeField]
    private bool m_viewGrabCircle = false;

    // Grab Circle Line - DEBUG USE ONLY
    private LineRenderer m_grabCircleLine;

    // Number of segments - DEBUG USE ONLY
    private int numSegments = 20;

    // Set the collision direction vector
    private Vector2 collisionDirection = Vector2.zero;

    // Saved height of the sprite
    private float spriteHeight;

    // Saved height of the sprite
    private float spriteWidth;

    // Player Status Enumerator
    public enum PlayerStatus
    {
        idle = 0,
        walking = 1,
        jumping = 2,
        falling = 3,
        swinging = 4,
        grabbing = 5
    }

    // Player status object
    [SerializeField]
    private PlayerStatus m_playerStatus;

    // Start is called before the first frame update
    private void Awake()
    {
        // Check the value held by the game objects tag
        if(tag == "Player_1")
        {
            // "Player_1" tag found, set the player index to 0
            m_playerIndex = 0;

            // Current player is player 1, obtain player 2
            m_otherPlayer = GameObject.FindGameObjectWithTag("Player_2").GetComponent<Character>();
        }
        else if (tag == "Player_2")
        {
            // "Player_2" tag found, set the player index to 1
            m_playerIndex = 1;

            // Current player is player 2, obtain player 1
            m_otherPlayer = GameObject.FindGameObjectWithTag("Player_1").GetComponent<Character>();
        }

        Debug.Log("Player " + (m_playerIndex + 1) + " GO: " + gameObject.GetInstanceID());

        // Obtain the 2D rigid body component
        m_rb = GetComponent<Rigidbody2D>();

        // Obtain bond component
        m_bond = GameObject.FindGameObjectWithTag("Bond").GetComponent<CharacterBond>();

        // Obtain the player animator component
        m_playerAnimator = GetComponent<Animator>();

        // Obtain player position
        m_playerPos = transform.position;

        // Initialise lastPlayer position
        m_lastPlayerPos = Vector2.zero;

        // Allow the player to move
        m_canMove = true;

        // DEBUG - Grab circle line renderer
        {
            // Add line renderer component to the gameobject
            m_grabCircleLine = gameObject.AddComponent<LineRenderer>();

            // Set the material of the grab circle line to the default sprite shader
            m_grabCircleLine.material = new Material(Shader.Find("Sprites/Default"));

            // Start colour for the grab circle
            m_grabCircleLine.startColor = Color.red;

            // End colour for the grab circle
            m_grabCircleLine.startColor = Color.red;

            // Start width
            m_grabCircleLine.startWidth = 0.2f;

            // End width
            m_grabCircleLine.endWidth = 0.2f;

            // Determine the number of positions for the line
            m_grabCircleLine.positionCount = numSegments + 1;

            // Set use world space to false
            m_grabCircleLine.useWorldSpace = false;

            // Invoke the debug Update grab circle method
            UpdateGrabCircle();

            // Check if view grab circle is disabled
            if (!m_viewGrabCircle)
            {
                // Disable the grab circle
                m_grabCircleLine.enabled = false;
            }
            else
            {
                // Enable the grab circle
                m_grabCircleLine.enabled = true;
            }
        }
        // Save the height and width of the sprite
        spriteHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y * 2;
        spriteWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
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
        // Check if player is within the grabable layer
        if (Physics2D.OverlapCircle(GetPlayerPosition(), m_grabRadius, m_grabbableLayer))
        {
            // Set can grab boolean to true
            m_canGrab = true;
        }
        else
        {
            // Set can grab boolean to false
            m_canGrab = false;
        }

        // DEBUG - View grab circle code
        {
            // DEBUG - Check if the view grab circle is enabled
            if (!m_viewGrabCircle)
            {
                // Disable the grab circle line renderer
                m_grabCircleLine.enabled = false;
            }
            else
            {
                // Enble the grab circle line renderer
                m_grabCircleLine.enabled = true;
            }

            // Update the grab circle
            UpdateGrabCircle();
        }

        // Determine half the height of the sprite
        float halfHeight = spriteHeight / 2;

        // Check if the player is grounded
        //m_isGrounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.1f), Vector2.down, 0.05f);

        // Check if the X input has a value above or less than 0
        if ((m_xInput < 0 || m_xInput > 0) && m_canMove)
        {
            // Flip sprite depending on value of X input
            GetComponent<SpriteRenderer>().flipX = -m_xInput < 0.0f;

            // Check if the player is not swinging
            if (!m_isSwinging)
            {
                Vector3 newPos = transform.position + new Vector3(m_xInput * m_movementSpeed * Time.deltaTime, 0, 0);
                // Move only when it is allowed
                if (m_bond.AllowableDistance(newPos, m_otherPlayer.transform.position))
                {
                    // Set the transform position to the new position
                    transform.position = newPos;
                }
                // Movement towards the other player is allowed too
                //else if (Vector2.Distance(newPos, m_otherPlayer.transform.position) < Vector2.Distance(transform.position, m_otherPlayer.transform.position))
                //{
                //    transform.position = newPos;
                //}
            }
            else
            {
                // Add force to the rigid body based on the player input
                m_rb.AddForce(new Vector2((m_xInput * m_movementSpeed - m_rb.velocity.x) * m_movementSpeed, 0));
                // Alter the rigid body velocity
                m_rb.velocity = new Vector2(m_rb.velocity.x, m_rb.velocity.y);
            }
        }

        // Correct the distance from each other in the air & enable distance joint
        if (Vector2.Distance(transform.position, m_otherPlayer.transform.position) >= m_bond.ReturnMaxLength())
        {
            m_bond.EnableDistanceJoint();
            m_bond.UpdateDistance(m_bond.ReturnMaxLength());
        }
        // if the distance is shorter and the play is airborn - disable the bond
        else if (!m_isGrounded && !m_touchingOtherPlayer)
        {
            m_bond.DisableDistanceJoint();
        }

        // Check if player is grounded or touching other player
        if (m_isGrounded || m_touchingOtherPlayer)
        {
            // Disable falling animation
            m_playerAnimator.SetBool("isFalling", false);

            // Check if there is an imput from the player
            if (m_xInput < 0 || m_xInput > 0)
            {
                // Enable the walking animation
                m_playerAnimator.SetBool("isWalking", true);
            }
            else
            {
                // Disable the walking animation
                m_playerAnimator.SetBool("isWalking", false);
            }

            // Check if player is at the right of the other player
            if (GetPlayerPosition().x >= m_otherPlayer.GetPlayerPosition().x)
            {
                // Player is moving to the right
                if (m_xInput > 0)
                {
                    // Extend the bond
                    m_bond.ExtendBond(Time.deltaTime * m_movementSpeed);
                }
                // Player is moving to the left
                else if (m_xInput < 0)
                {
                    // Shrink the bond
                    m_bond.ShrinkBond(Time.deltaTime * m_movementSpeed);
                }
            }

            // Check if the player is to the left of the other player
            if (GetPlayerPosition().x < m_otherPlayer.GetPlayerPosition().x)
            {
                // Player is moving to the right
                if (m_xInput > 0)
                {
                    // Shrink the bond
                    m_bond.ShrinkBond(Time.deltaTime * m_movementSpeed);
                }
                // Player is moving to the left
                else if (m_xInput < 0)
                {
                    // Extend the bond
                    m_bond.ExtendBond(Time.deltaTime * m_movementSpeed);
                }
            }

        }

        // Check if the player has walked off the edge of a platform
        if (!m_isFalling && !m_isGrounded && !m_touchingOtherPlayer && !m_isJumping)
        {
            // Set is falling to true
            m_isFalling = true;

            // Enable the distance joint
            m_bond.EnableDistanceJoint();
        }

        // Check if the player is jumping
        if (m_isJumping)
        {
            // Disable the walking animation
            m_playerAnimator.SetBool("isWalking", false);

            // Enable jumping animation
            m_playerAnimator.SetBool("isJumping", true);

            // Determine the player's movement direction
            Vector3 direction = transform.InverseTransformDirection(m_rb.velocity);

            // Check Y value of the direction vector
            if (direction.y < 0)
            {
                // Set is jumping to false
                m_isJumping = false;

                // Enable is falling
                m_isFalling = true;
            }
        }

        // Check if the player is falling and handle falling based actions
        if (m_isFalling)
        {
            // Enable falling animation
            m_playerAnimator.SetBool("isFalling", true);

            // Disable jumping animation
            m_playerAnimator.SetBool("isJumping", false);

            // Check if the player is falling below the other player
            if (GetPlayerPosition().y < m_otherPlayer.GetPlayerPosition().y && (m_otherPlayer.CheckGrabbing() || m_otherPlayer.CheckGrounded()))
            {
                // Set is falling to false
                m_isFalling = false;

                // Set is swinging to true
                m_isSwinging = true;

                // Enable the distance joint
                m_bond.EnableDistanceJoint();
            }

            // Check if the player is grounded or touching the other player
            if (m_isGrounded || m_touchingOtherPlayer)
            {
                // Player has hit the ground
                m_isFalling = false;

                // On ground disable the joint
                m_bond.DisableDistanceJoint();
            }
        }

        // Check if the player is swinging and handling swinging actions
        if (m_isSwinging)
        {
            // Check if Y input is less than 0 (holding down button)
            if (m_yInput < 0)
            {
                // Extend the bond
                m_bond.ExtendBond(Time.deltaTime * m_climbingSpeed);
            }

            // Check if Y input is greater than 0 (holding up button)
            if (m_yInput > 0)
            {
                // Shrink the bond
                m_bond.ShrinkBond(Time.deltaTime * m_climbingSpeed);
            }

            // Check if the player is grounded
            if (m_isGrounded)
            {
                // Set is swinging to false
                m_isSwinging = false;
            }
        }

        // Check if the player is grabbing
        if (m_isGrabbing)
        {
            // Make the rigid body become kinematic
            m_rb.isKinematic = true;

            // Remove any velocity from the player
            m_rb.velocity = new Vector2(0.0f, 0.0f);

            // Disable the players ability to move
            m_canMove = false;

            // Control the animation transition from grabbing
            m_playerAnimator.SetBool("isFalling", true);

            // And the other player is swinging enable the bond
            if (m_otherPlayer.m_isSwinging)
            {
                m_bond.EnableDistanceJoint();
            }
        }
        else
        {
            // Make the rigid body become dynamic by disabling the kinematic setting
            m_rb.isKinematic = false;

            // Enable the players ability to move
            m_canMove = true;
        }

        // Update the player position
        m_playerPos = transform.position;

        // Update the bond joint assigned to the player
        m_bond.UpdateJoint(GetPlayerIndex(), m_playerPos);

        // PLAYER STATUS

        // Check if the player is grounded and there is no movement from the player
        if (m_isGrounded && m_xInput == 0)
        {
            // Set player status to idle
            m_playerStatus = PlayerStatus.idle;
        }
        // Check if the player is grounded and there is movement from the player
        else if (m_isGrounded && m_xInput != 0)
        {
            // Set player status to walking
            m_playerStatus = PlayerStatus.walking;
        }
        // Check if the player is jumping
        else if (m_isJumping)
        {
            // Set player status to jumping
            m_playerStatus = PlayerStatus.jumping;
        }
        // Check if the player is falling
        else if (m_isFalling)
        {
            // Set the player status to falling
            m_playerStatus = PlayerStatus.falling;
        }
        // Check if the player is swinging
        else if (m_isSwinging)
        {
            // Set the player status to swinging
            m_playerStatus = PlayerStatus.swinging;
        }
        // Check if the player is grabbing
        else if (m_isGrabbing)
        {
            // Set the player status to grabbing
            m_playerStatus = PlayerStatus.grabbing;
        }
    }

    /*
     * SET X VALUE METHOD
     * 
     * Method for handling X input from
     * the player pressing the left or
     * right inputs
     */
    public void SetXValue(float xValue)
    {
        // Set the value of the X direction variable based on the value from the argument
        m_xInput = xValue;
    }

    /*
     * SET Y VALUE METHOD
     * 
     * Method for handling X input from
     * the player pressing the up or
     * down inputs
     */
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
        if (m_isGrounded || (m_touchingOtherPlayer && (m_otherPlayer.CheckGrounded() || m_otherPlayer.CheckGrabbing())))
        {
            // Invoke the jump action in Y, whilst keeping the rigid body velocity in X
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);

            // Set is jumping to true
            m_isJumping = true;

            // Disable the distance joint
            m_bond.DisableDistanceJoint();
        }
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

            // Save collison vector from the center of the players feet
            collisionDirection = collision.GetContact(0).point 
                - new Vector2(transform.position.x, transform.position.y - spriteHeight / 2);
        }

        // Check if the player is touching other player (11 is the environment layer)
        if (collision.gameObject.layer == 10)
        {
            // Set touching other player to true
            m_touchingOtherPlayer = true;

            // Save collison vector from the center of the players feet
            collisionDirection = collision.GetContact(0).point 
                - new Vector2(transform.position.x, transform.position.y - spriteHeight / 2);
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

        // Check if the player is touching other player (10 is the player layer)
        if (collision.gameObject.layer == 10)
        {
            // Set isTouchingOtherPlayer to true
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
     * GET PLAYER POSITION METHOD
     * 
     * Method returns the value held
     * by the isGrounded variable.
     */     
    public Vector2 GetPlayerPosition()
    {
        return m_playerPos;
    }

    /*
     * GRAB METHOD
     * 
     * Method for handling the player when
     * they are grabbing
     */
    public void Grab()
    {
        // Check if the player can grab
        if (m_canGrab)
        {
            // Set is grabbing to true
            m_isGrabbing = true;

            // Set other booleans to false
            m_isFalling = false;
            m_isGrounded = false;
            m_isJumping = false;
            m_isSwinging = false;

            // Set player status to grabbing
            m_playerStatus = PlayerStatus.grabbing;

            // Control the animation
            m_playerAnimator.SetBool("isWalking", false); // it doesn't walk anymore
            m_playerAnimator.SetBool("isJumping", false); // it's not jumping anymore

            // Animate the grab
            m_playerAnimator.SetBool("isGrabbing", true);

            // If the collision is close to the feet it is grabbing the floor
            if (collisionDirection.magnitude <= spriteWidth / 3)
            {
                m_playerAnimator.SetBool("isGrabbingFloor", true);
            }
            else
            {
                m_playerAnimator.SetBool("isGrabbingFloor", false);
            }
        }
    }

    /*
     * GRAB METHOD
     * 
     * Method for handling the player when
     * they are grabbing
     */
    public void Release()
    {
        // Set is grabbing to false
        m_isGrabbing = false;

        // Stop animating the grab
        m_playerAnimator.SetBool("isGrabbing", false);

        // The other player is not swinging anymore (if they were)
        m_otherPlayer.SetSwinging(false);
    }

    /*
     * UPDATE GRAB CIRCLE METHOD - DEBUG ONLY
     * 
     * Debug method update the positions of the grab
     * circle for the player.
     */
    private void UpdateGrabCircle()
    {
        // Determine delta theta
        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;

        // Start with theta = 0.0f
        float theta = 0.0f;

        // Iterate over the number of segments
        for (int i = 0; i < numSegments + 1; i++)
        {
            // Determine X coordinate
            float x = m_grabRadius * Mathf.Cos(theta);

            // Determine Y coordinate
            float y = m_grabRadius * Mathf.Sin(theta);

            // Set point of the circle
            Vector3 pointPosition = new Vector3(x, y, 0);

            // Set the position of the point for the grab circle
            m_grabCircleLine.SetPosition(i, pointPosition);

            // Increment theta by delta theta
            theta += deltaTheta;
        }
    }
    // Method to set the isSwinging
    public void SetSwinging(bool var)
    {
        m_isSwinging = var;
    }
    // Method to get isGrabbing
    public bool CheckGrabbing()
    {
        return m_isGrabbing;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return m_playerStatus;
    }
}