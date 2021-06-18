using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayersBond bond = null;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int jumpPower = 10;
    public int playerId = 0;
    private int canJump = 0;
    private Vector3 newPos;

    private void Awake() {
        // Increase the player id based on the player manager
        PlayersManager.IncreasePlayerCount();
        playerId = PlayersManager.GetPlayerCount();
    }
    private void Update()
    {
        // Get the input keys from the horizontal axis based on playerId
        var movement =  Input.GetAxis("Horizontal" + playerId.ToString());
        // Respect the maximum bond length
        newPos = transform.position + new Vector3(movement, 0, 0) * Time.deltaTime * speed;
        if (bond.isAllowedDistance(newPos, playerId))
        {
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;
        }

        // Detect jumping
        if (Input.GetButtonDown("Jump" + playerId.ToString()) && canJump == 1) 
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * jumpPower), ForceMode2D.Impulse);
        }
    }
    // collisiond handling inspired from: https://answers.unity.com/questions/1220752/how-to-detect-if-not-colliding.html
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 10) // 11 is the environment layer & 10 is players
        {
            canJump++;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 10)
        {
            canJump--;
        }
    }
}
