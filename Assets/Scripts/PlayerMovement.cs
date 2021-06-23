using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayersBond bond = null;
    [SerializeField] private PlayerMovement otherPlayer = null;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int jumpPower = 10;
    public int playerId = 0;
    private bool playerTouch = false;
    private bool groundTouch = false;
    private Vector3 newPos;
    private Vector3 bondClimbing = new Vector3(0, 0, 0);
    public AK.Wwise.Event playerJumpSound; // declaring Wwise-Type Event 
    private void Awake() {
        // Increase the player id based on the player manager
        PlayersManager.IncreasePlayerCount();
        playerId = PlayersManager.GetPlayerCount();
    }
    private void Update()
    {
        // Get the input keys from the horizontal & vertical axis based on playerId
        var hMovement =  Input.GetAxis("Horizontal" + playerId.ToString());
        var vMovement =  Input.GetAxis("Vertical" + playerId.ToString());
        if (vMovement != 0)
        {   
            // The Vertical Axis allow players to climb towards the other player
            bondClimbing = playerId == 1 ? - bond.playersVector / bond.playersVector.magnitude * vMovement
                : bond.playersVector / bond.playersVector.magnitude * vMovement;
        }
        newPos = transform.position + (new Vector3(hMovement, 0, 0) + bondClimbing) * Time.deltaTime * speed;
        // Respect the maximum bond length
        if (bond.isAllowedDistance(newPos, playerId))
        {
            if (playerId == 2) 
            {
                // Update the joint's length
                GetComponent<DistanceJoint2D>().distance = bond.playersVector.magnitude;
            }
            transform.position = newPos;
        }
        // Correct the distance from each other in the air
        else if (!(playerTouch || groundTouch) && playerId == 2)
        {
            GetComponent<DistanceJoint2D>().distance = bond.maxLength;
        }
        // Disable distance joint when up close to avoid side effects
        if (playerId == 2)
        {
            GetComponent<DistanceJoint2D>().enabled = bond.playersVector.magnitude <= 5 ? false : true;
        }
        // Detect jumping
        if (Input.GetButtonDown("Jump" + playerId.ToString()) && (playerTouch || groundTouch)) 
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * jumpPower), ForceMode2D.Impulse);
            AkSoundEngine.PostEvent("play_jump_1",gameObject); //posting event on jump 
        }
    }
    // collisiond handling inspired from: https://answers.unity.com/questions/1220752/how-to-detect-if-not-colliding.html
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 11) // 11 is the environment layer
        {
            groundTouch = true;
        }
        if (collision.gameObject.layer == 10 && otherPlayer.groundTouch) // layer 10 is players
        {
            playerTouch = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == 11)
        {
            groundTouch = false;
        }
        if (collision.gameObject.layer == 10 && otherPlayer.groundTouch)
        {
            playerTouch = false;
        }
    }
}
