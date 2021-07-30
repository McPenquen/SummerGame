using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject selectionSprite = null;
    // Control variable to play the sound single time
    private bool isSoundPlayed = false;
    void Update()
    {
        // Show the selection sprite when it's selected
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            selectionSprite.SetActive(true);
            // Play selection sound
            if (!isSoundPlayed)
            {
                isSoundPlayed = true;
                AkSoundEngine.PostEvent("play_ui_move", transform.parent.gameObject);
            }
        }
        else
        {
            selectionSprite.SetActive(false);
            isSoundPlayed = false;
        }
    }
    
}
