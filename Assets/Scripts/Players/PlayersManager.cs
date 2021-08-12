using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private GameObject player1 = null;
    [SerializeField] private GameObject player2 = null;
    [SerializeField] private GameObject bond = null;
    public static PlayersManager instance = null;
    void Awake() {
        if (instance == null) 
        {
            instance = this;
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
    }
    // Singleton inspired from: https://www.studica.com/blog/how-to-create-a-singleton-in-unity-3d
    public void ChangePosition(Vector3 newPos)
    {
        // player 2
        player1.transform.position = new Vector3(newPos.x - 2, newPos.y, 0);
        // player 1
        player2.transform.position = new Vector3(newPos.x + 2, newPos.y, 0);
        // bond
        bond.transform.position = newPos;

    }
    // Return position of the bond
    public Vector3 GetPosition()
    {
        return bond.transform.position;
    }
    // Get position of player 1
    public Vector3 GetPlayer1Position()
    {
        return player1.transform.position;
    }
    // Get position of player 2
    public Vector3 GetPlayer2Position()
    {
        return player2.transform.position;
    }
}
