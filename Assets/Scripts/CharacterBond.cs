using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBond : MonoBehaviour
{
    // PLAYER VARIABLES
    [Header("Player Settings")]

    // Player 1
    [SerializeField] 
    private Character m_player1;

    // Player 2
    [SerializeField] 
    private Character m_player2;

    // Maximum Length
    [SerializeField] 
    private float m_maxLength = 10.0f; // maximum allowed length of the bond

    // Offset from player center
    private float yOffset = -0.5f; // offset from the players' centres
    
    // Bond joint position of player 1
    private Vector3 p1Joint = Vector3.zero;

    // Bond joint position of player 2
    private Vector3 p2Joint = Vector3.zero;

    // BOND SETTINGS
    [Header("Bond Settings")]

    // Bond Centre - Where the camera will focus to
    private Vector3 m_bondCentre = Vector3.zero;

    // Bond Scale
    private Vector3 m_bondScale;

    // Bond Scale Factor
    [SerializeField, Range(1.0f, 12.0f)] 
    private float m_bondScaleFactor = 9.0f;

    // DEBUG LINE SETTINGS
    [Header("Debug Line Settings")]

    // Debug line view boolean
    [SerializeField]
    private bool m_debugLineView = false;

    // Debug line width
    [SerializeField]
    private float m_debugLineWidth = 0.2f;

    // Debug Line Renderer
    private LineRenderer m_debugLine;

    [SerializeField]
    private float m_bondLength;

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
        p1Joint = m_player1.transform.position + new Vector3(0.0f, yOffset, 0.0f);
        p2Joint = m_player2.transform.position + new Vector3(0.0f, yOffset, 0.0f);

        // Obtain the bond centre
        m_bondCentre = (p1Joint + p2Joint) / 2.0f;

        // Obtain the bond scale
        m_bondScale = transform.localScale;

        // Add line renderer component
        m_debugLine = gameObject.AddComponent<LineRenderer>();

        // Set start point
        m_debugLine.SetPosition(0, p1Joint);

        // Set end point
        m_debugLine.SetPosition(1, p2Joint);

        // Set start width of line 
        m_debugLine.startWidth = m_debugLineWidth;

        // Set end width of line
        m_debugLine.endWidth = m_debugLineWidth;
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
        // Update position of bond joints for both players
        p1Joint = m_player1.transform.position + new Vector3(0.0f, yOffset, 0.0f);
        p2Joint = m_player2.transform.position + new Vector3(0.0f, yOffset, 0.0f);

        // Determine the centre of the bond
        m_bondCentre = (p1Joint + p2Joint)/ 2.0f;

        // Set the bond position
        transform.position = m_bondCentre;

        // Determine the direction 
        Vector3 bondDirection = p2Joint - p1Joint;

        m_bondLength = Vector3.Distance(p1Joint, p2Joint);

        // Set the right transform to the bond direction
        transform.right = bondDirection;

        // Determine the bond scale of the x component
        m_bondScale.x = Vector3.Distance(p1Joint, p2Joint) / (m_maxLength - (0.1f * m_maxLength));

        // Set the local scale to the bond scale
        transform.localScale = m_bondScale;

        // Update the positions of the debug line
        m_debugLine.SetPosition(0, p1Joint);
        m_debugLine.SetPosition(1, p2Joint);

        // Check if debug line boolean is true and debug line is disabled
        if(m_debugLineView == true && m_debugLine.enabled == false)
        {
            // Enable the debug line
            m_debugLine.enabled = true;
        }
        // Else, check if debug line boolean is false and debug line is enabled
        else if(m_debugLineView == false && m_debugLine.enabled == true)
        {
            // Disable the debug line
            m_debugLine.enabled = false;
        }
    }

    /*
     * ALLOWABLE DISTANCE METHOD
     * 
     * Method is used to check if the players
     * have exceeded the maximum bond distance
     * or not.
     */
    public bool AllowableDistance(Vector3 playerPos, Vector3 otherPlayerPos)
    {
        // Determine the distance between both players
        float distancePlayers = Vector3.Distance(playerPos, otherPlayerPos);

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
     * GET BOND VECTOR METHOD
     * 
     * 
     */
    public Vector3 GetBondVector()
    {
        return p1Joint - p2Joint;
    }

    /*
     * GET DISTANCE METHOD
     * 
     * 
     */
    public float GetDistance()
    {
        return Vector3.Distance(p1Joint, p2Joint);
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

}
