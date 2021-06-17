using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBond : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private float zLayer = 1.0f;
    private Vector3 newPosition = new Vector3(0, 0, 0);
    private Quaternion newRotation = new Quaternion(0, 0, 0, 0);
    private Vector3 playersVector;
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
}
