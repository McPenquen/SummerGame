using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // Boolean if the players have reached the checkpoint
    private bool _isReached = false;
    // Animator
    [SerializeField] private Animator checkpointAnimator = null;
    private void Start()
    {
        // Save the reference to the animator component
        checkpointAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // if is reached set the new animation
        if (_isReached)
        {
            checkpointAnimator.SetBool("isOn", true);
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
