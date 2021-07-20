using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBond : MonoBehaviour
{
    // OBJECT REFERENCES 
    [Header("Object References")]

    // Player 1 object
    [SerializeField] 
    private Character m_player1;

    // Player 2 object
    [SerializeField] 
    private Character m_player2;

    // Distance joint
    [SerializeField]
    private DistanceJoint2D m_distanceJoint;

    // BOND VARIABLES
    [Header("Bond Settings")]

    // Maximum Bond Length
    [SerializeField] 
    private float m_maxLength = 10.0f;

    // Offset from player center
    [SerializeField]
    private float yOffset = 0.5f;

    // BOND VALUES
    [Header("Bond Values")]

    // Bond Centre - Where the camera will focus to
    [SerializeField]
    private Vector2 m_bondCentre;

    // Bond joint position of player 1
    [SerializeField]
    private Vector2 m_p1Joint = Vector2.zero;

    // Bond joint position of player 2
    [SerializeField]
    private Vector2 m_p2Joint = Vector2.zero;

    // BOND RENDERER SETTINGS
    [Header("Bond Renderer Settings")]
    // Debug Line Renderer
    [SerializeField]
    private LineRenderer m_bondRenderer;

    // Debug line width
    [SerializeField]
    private float m_bondThickness = 1.0f;

    // Bond length
    [SerializeField]
    private float m_bondLength = 5.0f;

    /*
     * AWAKE METHOD
     * 
     * Standard Unity method.
     * 
     * Method first obtains references to both
     * players, then determines the joint
     * positions for both players.
     * 
     * Method then botains the centre and scale
     * values for the bond.
     * 
     * Method also establishes a new line renderer
     * component and initialises it with a few
     * values.
     */
    private void Awake()
    {
        // Obtain player 1, based on the "Player_1" tag
        m_player1 = GameObject.FindGameObjectWithTag("Player_1").GetComponent<Character>();

        // Obtain player 2, based on the "Player_2" tag
        m_player2 = GameObject.FindGameObjectWithTag("Player_2").GetComponent<Character>();

        // Update position of bond joints for both players
        m_p1Joint = m_player1.GetPlayerPosition() + new Vector2(0.0f, -yOffset);
        m_p2Joint = m_player2.GetPlayerPosition() + new Vector2(0.0f, -yOffset);
        
        // Obtain the bond centre
        m_bondCentre = (m_p1Joint + m_p2Joint) / 2.0f;

        // Obtain the distance joint attached to player 1
        m_distanceJoint = m_player1.GetComponent<DistanceJoint2D>();

        // Set the anchor point to player 1
        m_distanceJoint.anchor = m_p1Joint;

        // Set player 2 as the connected body
        m_distanceJoint.connectedBody = m_player2.GetComponent<Rigidbody2D>();

        // Set the connected anchor point to player 2
        m_distanceJoint.connectedAnchor = m_p2Joint;

        // Update the bond length between p1 and p2 joints
        m_bondLength = Vector3.Distance(m_p1Joint, m_p2Joint);

        // Set the distance of the distance joint to the bond length
        m_distanceJoint.distance = m_maxLength;

        // Add line renderer component
        m_bondRenderer = gameObject.GetComponent<LineRenderer>();

        // Set the bond position based on the values obtained for p1 and p2 joints
        SetBondRendererPosition(m_p1Joint, m_p2Joint);

        // Set the bond thickness
        SetBondWidth(m_bondThickness);
    }
    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is updated
     * once per frame.
     * 
     * Method updates the position of the player
     * joints and determines the centre of the 
     * bond. Method then updates
     */
    void Update()
    {
        // Determine the centre of the bond
        m_bondCentre = (m_p1Joint + m_p2Joint) / 2.0f;

        // Update the bond transform position
        transform.position = m_bondCentre;

        // Update the bond length between p1 and p2 joints
        m_bondLength = Vector3.Distance(m_p1Joint, m_p2Joint);

        // Set the distance of the distance joint to the bond length
        m_distanceJoint.distance = m_bondLength;

        // Update the bond position
        SetBondRendererPosition(m_p1Joint, m_p2Joint);

        // Update the bond thickness
        SetBondWidth(m_bondThickness);
    }

    /*
     * ALLOWABLE DISTANCE METHOD
     * 
     * Method is used to check if the players
     * have exceeded the maximum bond distance
     * or not.
     */
    public bool AllowableDistance(Vector2 playerPos, Vector2 otherPlayerPos)
    {
        // Determine the distance between both players
        float distancePlayers = Vector2.Distance(playerPos, otherPlayerPos);

        // Check if the distance between the players is less than, or equal to, the maximum bond length
        if(distancePlayers <= m_maxLength)
        {
            // Less than maximum bond length, return true
            return true;
        }
        else
        {
            // Greater than the maximum bond length, return false
            return false;
        }
    }

    /*
     * RETURN MAX LENGTH METHOD
     * 
     * Public method that returns the
     * maximum length of the bond
     */
    public float ReturnMaxLength()
    {
        // Return maximum length
        return m_maxLength;
    }

    /*
     * SET BOND RENDER POSITION
     * 
     * Method for setting the start and end
     * points for the bond renderer
     */
    private void SetBondRendererPosition(Vector3 startPos, Vector3 endPos)
    {
        // Set the start position
        m_bondRenderer.SetPosition(0, startPos);

        // Set the end position
        m_bondRenderer.SetPosition(1, endPos);
    }

    /*
     * SET BOND WIDTH METHOD
     * 
     * Method for setting the width of the
     * line renderer for the bond between
     * both players
     */
    private void SetBondWidth(float width)
    {
        // Set the start width
        m_bondRenderer.startWidth = width;

        // Set the end width
        m_bondRenderer.endWidth = width;
    }

    /*
     * UPDATE JOINT METHOD
     * 
     * Method for updating the bond joint
     * depending on the player's index
     * number
     */
    public void UpdateJoint(int playerIdx, Vector2 playerPos)
    {
        // Check if player index is 0 (player 1)
        if(playerIdx == 0)
        {
            // Update
            m_p1Joint = playerPos + new Vector2(0.0f, -yOffset);
        }
        // Else player 2
        else
        {
            m_p2Joint = playerPos + new Vector2(0.0f, -yOffset);
        }
    }

    /*
     * ENABLE THE DISTANCE JOINT
     * 
     * Method for enabling the distance joint
     */
    public void EnableDistanceJoint()
    {
        // Enable the distance joint
        m_distanceJoint.enabled = true;
    }

    /*
     * DISABLE THE DISTANCE JOINT METHOD
     * 
     * Method for disabling the distance joint
     */
    public void DisableDistanceJoint()
    {
        // Disable the distance joint
        m_distanceJoint.enabled = false;
    }

    /*
     * SHRINK BOND METHOD
     * 
     * Method for shrinking the distance
     * joint distance
     */
    public void ShrinkBond(float shrink)
    {
        // Decrement the distance by a shrink value
        m_distanceJoint.distance -= shrink;
    }

    /*
     * EXTEND BOND METHOD
     * 
     * Method is used to extend the distance joint
     * provided that it will not cause the distance
     * to exceed the maximum length
     */
    public void ExtendBond(float extend)
    {
        // Check if the current distance + the extend value is less than the max length
        if(m_distanceJoint.distance + extend < m_maxLength)
        {
            // Increment the distance by an extend valye
            m_distanceJoint.distance += extend;
        }
    }
}
