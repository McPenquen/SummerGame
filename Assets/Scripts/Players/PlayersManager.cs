using UnityEngine;

public class PlayersManager : MonoBehaviour
{
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
        transform.GetChild(0).position = new Vector3(newPos.x - 2, newPos.y, 0);
        // player 1
        transform.GetChild(1).position = new Vector3(newPos.x + 2, newPos.y, 0);
        // bond
        transform.GetChild(2).position = newPos;

    }
    // Return position of the bond
    public Vector3 GetPosition()
    {
        return transform.GetChild(2).position;
    }
}
