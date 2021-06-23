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
}
// Singleton inspired from: https://www.studica.com/blog/how-to-create-a-singleton-in-unity-3d

