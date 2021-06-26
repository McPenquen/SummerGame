using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayersBond bond = null;
    [SerializeField] private PlayerMovement otherPlayer = null;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int jumpPower = 10;
    public int playerId = 0;
    [SerializeField] private bool playerTouch = false;
    [SerializeField] private bool groundTouch = false;
    private Vector3 newPos = new Vector3(0, 0, 0);
    private Vector3 bondClimbing = new Vector3(0, 0, 0);
    private Animator animator = null;

    private void Awake() {
        // Increase the player id based on the player tag
        if (tag == "Player_1")
        {
            playerId = 1;
        }
        else
        {
            playerId = 2;
        }

        // Initialise animator
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // Get the input keys from the horizontal & vertical axis based on playerId
        var hMovement =  Input.GetAxis("Horizontal" + playerId.ToString());
        var vMovement =  Input.GetAxis("Vertical" + playerId.ToString());
        if (vMovement != 0)
        {   
            // The Vertical Axis allow players to climb towards the other player -> up = extend , down = shrink the bond
            bondClimbing = playerId == 1 ? bond.playersVector / bond.playersVector.magnitude * vMovement
                : - bond.playersVector / bond.playersVector.magnitude * vMovement;
            
        }

        newPos = transform.position + (new Vector3(hMovement, 0, 0) + bondClimbing) * Time.deltaTime * speed;

        // Respect the maximum bond length
        if (bond.isAllowedDistance(newPos, playerId))
        {
            // Animate walking
            if (hMovement != 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (playerId == 2) 
            {
                // Update the joint's length
                GetComponent<DistanceJoint2D>().distance = bond.playersVector.magnitude;
            }

            // Update position
            transform.position = newPos;
        }

        // Correct the distance from each other in the air
        else if (!(playerTouch || groundTouch))
        {
            if (playerId == 2)
            {
                GetComponent<DistanceJoint2D>().distance = bond.maxLength;
            }
            else 
            {
                otherPlayer.GetComponent<DistanceJoint2D>().distance = bond.maxLength;
            }
        }

        // Disable distance joint when up close to avoid side effects of the distance joint
        if (playerId == 2)
        {
            if (bond.playersVector.magnitude <= 5)
            {
                // If one of the players is hanging enable the bond
                if ((transform.position.y < otherPlayer.transform.position.y && (!playerTouch && !groundTouch)) 
                    || (otherPlayer.transform.position.y < transform.position.y && (!otherPlayer.playerTouch && !otherPlayer.groundTouch))
                )
                {
                    GetComponent<DistanceJoint2D>().enabled = true;
                }
                else
                {
                    GetComponent<DistanceJoint2D>().enabled = false;
                }
            }
            else
            {
                GetComponent<DistanceJoint2D>().enabled = true;
            }
        }

        // Detect jumping
        if (Input.GetButtonDown("Jump" + playerId.ToString()) && ((playerTouch && otherPlayer.groundTouch) || groundTouch)) 
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * jumpPower), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        // Animate jumping and falling based on the direction of the movement
        if (!groundTouch && !playerTouch)
        {
            Vector3 direction =  transform.InverseTransformDirection(GetComponent<Rigidbody2D>().velocity); //transform.position - previousPos;
            if (direction.y <= 0)
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", true);
            }
        }
        else 
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
    }
    // collisiond handling inspired from: https://answers.unity.com/questions/1220752/how-to-detect-if-not-colliding.html
    // direction of movement from: https://answers.unity.com/questions/689999/how-to-determine-the-direction-an-object-is-moving.html
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
