using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance = null;
    [SerializeField] static int playerCount = 0;
    void Awake() {
        if (instance == null) 
        {
            instance = this;
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
        // Reset the player count to 0
        playerCount = 0;
    }
    // New methods
    public static void IncreasePlayerCount()
    {
        playerCount++;
    }
    public static int GetPlayerCount()
    {
        return playerCount;
    } 
}
// Singleton inspired from: https://www.studica.com/blog/how-to-create-a-singleton-in-unity-3d

