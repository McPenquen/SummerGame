using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // Boolean if the players have reached the checkpoint
    private bool _isReached = false;

    void Update()
    {
        // if is reached set colour
        if (_isReached)
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    // Return if players have passed by this checkpoint
    public bool isReached()
    {
        return _isReached;
    }
    
    // Set _isReached
    public void SetIsReached(bool b) 
    {
        _isReached = b;
    }
    
}
