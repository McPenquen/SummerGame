using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAudioManager : MonoBehaviour
{
    // Serialize Field
    [SerializeField]
    private Character m_player;

    [SerializeField]
    private Character.PlayerStatus playerStatus;

    [SerializeField]
    private Character.PlayerStatus previousStatus;

    // Character name string
    [SerializeField]
    private string characterName = "";

    // Character state
    string characterState = "";

    // Play Event string
    string playEvent = "";

    // Stop event string
    string stopEvent = "";

    // GameObject
    GameObject go;

    private void Awake()
    {
        // Obtain the character component attached to the game object
        m_player = GetComponent<Character>();

        // Obtain the player status from the player
        playerStatus = GetComponent<Character>().GetPlayerStatus();

        // Establish character state string
        characterState = characterName + "_Movement";

        // Establish play event string
        playEvent = "play_" + characterName.ToLower() + "_movement_switch_state";

        // Establish stop event string
        stopEvent = "stop_" + characterName.ToLower() + "_movement_switch_state";

        // Obtain the game object
        go = this.gameObject;

        // Set the previous status to the player status
        previousStatus = playerStatus;
    }


    private void Update()
    {
        // Update the player status
        playerStatus = m_player.GetPlayerStatus();

        // Check for status change
        if (playerStatus != previousStatus)
        {
            // Status has changed


            // Check the player status
            if (playerStatus == Character.PlayerStatus.idle)
            {
                // Play Idle sounds
                AkSoundEngine.SetState(characterState, "idle");
            }
            else if (playerStatus == Character.PlayerStatus.walking)
            {

                // Play walking sounds for the character
                AkSoundEngine.SetState(characterState, "isWalking");
            }
            else if (playerStatus == Character.PlayerStatus.jumping)
            {
                // Play jumping sounds for the character
                AkSoundEngine.SetState(characterState, "isJumping");
            }
            else if (playerStatus == Character.PlayerStatus.falling)
            {
                // Play falling sound for the character
                AkSoundEngine.SetState(characterState, "isFalling");
            }
            else if (playerStatus == Character.PlayerStatus.swinging)
            {
                // Play swinging sound for the character
                AkSoundEngine.SetState(characterState, "isSwinging");
            }
            else if (playerStatus == Character.PlayerStatus.grabbing)
            {
                // Play grabbing sound for the character
                AkSoundEngine.SetState(characterState, "isGrabbing");
            }
            else
            {
                // No sound for the character
                AkSoundEngine.SetState(characterState, "None");
            }


            // Post Sound event to sound engine
            AkSoundEngine.PostEvent(playEvent, go);

            previousStatus = playerStatus;
        }


    }
}
