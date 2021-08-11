using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    // Serialize Field
    [SerializeField]
    private Character m_player;

    [SerializeField]
    private Character.PlayerStatus playerStatus;

    // Character name string
    string characterName = "";

    // Character state
    string characterState = "";

    // Play Event string
    string playEvent = "";

    // Stop event string
    string stopEvent = "";

    // GameObject
    GameObject go;


    private enum SoundPlaying
    {
        none,
        idle,
        walking,
        jumping,
        swinging,
        falling,
        grabbing
    }

    private SoundPlaying soundEvent;

    private void Awake()
    {
        // Obtain the character component attached to the game object
        m_player = GetComponent<Character>();

        // Obtain the player status from the player
        playerStatus = GetComponent<Character>().GetPlayerStatus();

        // Assign the switch state according to the player index value
        if (GetComponent<Character>().GetPlayerIndex() == 0)
        {
            // Set player 1
            characterName = "Ini";
        }
        else if (GetComponent<Character>().GetPlayerIndex() == 1)
        {
            // Set player 2
            characterName = "Gem";
        }

        // Establish character state string
        characterState = characterName + "_Movement";

        // Establish play event string
        playEvent = "play_" + characterName.ToLower() + "_movement_switch_state";

        // Establish stop event string
        stopEvent = "stop_" + characterName.ToLower() + "_movement_switch_state";

        // Initialise sound event to none
        soundEvent = SoundPlaying.none;

        go = this.gameObject;

    }

    private void Update()
    {
        // Update the player status
        playerStatus = m_player.GetPlayerStatus();

        // Post Sound event to sound engine
        AkSoundEngine.PostEvent(playEvent, go);

        


        // Check the player status
        if (playerStatus == Character.PlayerStatus.idle)
        {
            // Play Idle sounds
            //AkSoundEngine.SetState(characterState, "idle");
        }
        else if(playerStatus == Character.PlayerStatus.walking)
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

            soundEvent = SoundPlaying.none;
        }












    }

}
