using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBond : MonoBehaviour
{
    [SerializeField] private PlayerMovement player1;
    [SerializeField] private PlayerMovement player2;
    [SerializeField] private float zLayer = 1.0f;
    [SerializeField] public float maxLength = 10.0f;
    private Vector3 newPosition = new Vector3(0, 0, 0);
    private Quaternion newRotation = new Quaternion(0, 0, 0, 0);
    public Vector3 playersVector;
    private Vector3 bondScale = new Vector3(3, 0.5f, 1);

    void Update()
    {
        // Update the length of the bond
        playersVector = player1.transform.position - player2.transform.position;
        bondScale.x = playersVector.magnitude;
        transform.localScale = bondScale;

        // Update the bond's bosition
        newPosition = player1.transform.position + player2.transform.position;
        newPosition = newPosition / 2;
        newPosition.z = zLayer;
        transform.position = newPosition;

        // Rotate the bond
        newRotation = Quaternion.LookRotation(player1.transform.position - player2.transform.position);
        newRotation.y = 0;
        newRotation.x = 0;
        transform.rotation = newRotation;
    }

    // Takes in a Vector3 - if the new position is within the range return true else false
    public bool isAllowedDistance(Vector3 position, int playerId)
    {
        // Get the distance from the other player
        float newPlayersDistance = playerId == player1.playerId ? 
        (player2.transform.position - position).magnitude : (player1.transform.position - position).magnitude;
        // Return if the distance is allowed
        return newPlayersDistance <= maxLength;
    }
}
