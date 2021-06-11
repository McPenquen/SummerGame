using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public int playerId = 1;
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public int jumpPower = 10;

    private void Update()
    {
        // Set the movement based on the playerID
        if (playerId == 1)
        {
            var movement =  Input.GetAxis("Horizontal1");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;
            // Handle jumping
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * jumpPower), ForceMode2D.Impulse);
            }
        }
        else 
        {
            var movement = Input.GetAxis("Horizontal2");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;
            // Handle jumping
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * jumpPower), ForceMode2D.Impulse);
            }
        }
    }
}
