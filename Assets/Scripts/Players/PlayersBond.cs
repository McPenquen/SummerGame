using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBond : MonoBehaviour
{
    [SerializeField] private PlayerMovement player1;
    [SerializeField] private PlayerMovement player2;
    [SerializeField] public float maxLength = 10.0f; // maximum allowed length of the bond
    private Vector3 newPosition = new Vector3(0, 0, 0);
    private Quaternion newRotation = new Quaternion(0, 0, 0, 0);
    public Vector3 playersVector; // vector from the joint to player 2 to player 1's joint
    private Vector3 bondScale = new Vector3(0.45f, 0.3f, 1); // the bond's set y and z value
    private float yOffset = -0.5f; // offset from the players' centres
    // Positions of the bond connecting to the players
    private Vector3 p1Joint = new Vector3(0, 0, 0);
    private Vector3 p2Joint = new Vector3(0, 0, 0);

    void Update()
    {
        // Calculate the new positions of the joints
        p1Joint = new Vector3(player1.transform.position.x, player1.transform.position.y + yOffset, player1.transform.position.z);
        p2Joint = new Vector3(player2.transform.position.x, player2.transform.position.y + yOffset, player2.transform.position.z);

        // Update the length of the bond
        playersVector = p1Joint - p2Joint;
        bondScale.x = playersVector.magnitude / 9;
        transform.localScale = bondScale;

        // Update the bond's bosition
        newPosition = p1Joint + p2Joint;
        newPosition = newPosition / 2;
        transform.position = newPosition;

        // Rotate the bond
        newRotation = Quaternion.LookRotation(p1Joint - p2Joint);
        newRotation.y = 0;
        newRotation.x = 0;
        transform.rotation = newRotation;
    }

    // Takes in a Vector3 - if the new position is within the range return true else false
    public bool isAllowedDistance(Vector3 position, int playerId)
    {
        // Get the distance from the other player
        float newPlayersDistance = playerId == player1.playerId ? 
        (p2Joint - position).magnitude : (p1Joint - position).magnitude;
        // Return if the distance is allowed
        return newPlayersDistance <= maxLength;
    }
}
