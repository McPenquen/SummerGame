using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectPlayerManager : MonoBehaviour
{
    // Instructions manager
    [SerializeField] InstructionsManager instructionsManager;
    // Counter of players that have been joined
    private int playersJoinedCounter = 0;
    // Bool if we have finished selecting a player
    private bool isSelectingPlayersFinished = false;
    // Player 1 selecting visual
    [SerializeField] GameObject player1SelectingVisual;
    // Player 2 selecting visual
    [SerializeField] GameObject player2SelectingVisual;
    // Object with player manager
    [SerializeField] GameObject playerInputManagerObject;
    // Player manager component
    private PlayerInputManager playerInputManager;
    // Players
    [SerializeField] private PlayersManager players;

    private void Start()
    {
        // Player 2 visual is not there yet
        player2SelectingVisual.SetActive(false);
        // Player 1 is selecting first
        player1SelectingVisual.SetActive(true);
        
        // Save player input manager component
        playerInputManager = playerInputManagerObject.GetComponent<PlayerInputManager>();
    }
    void Update()
    {
        if (!isSelectingPlayersFinished)
        {
            // Extend the instruction until both players have joined
            if (playersJoinedCounter < 2)
            {
                instructionsManager.ExtendInstructionsTimer();

                // Set the number of players detected
                playersJoinedCounter = playerInputManager.playerCount;

                // after 1st player the second player is chosing
                if (playersJoinedCounter == 1)
                {
                    player1SelectingVisual.SetActive(false);
                    player2SelectingVisual.SetActive(true);
                }

                // Follow the height of the bond for the visuals
                player1SelectingVisual.transform.position = players.GetPlayer1Position();
                player2SelectingVisual.transform.position = players.GetPlayer2Position();
            }
            // If we have 2 players we can end the intruction display and selecting the players is done
            else if (playersJoinedCounter == 2)
            {
                instructionsManager.EndCurrentinstruction();
                isSelectingPlayersFinished = true;
                // Make sure both visuals are off
                player1SelectingVisual.SetActive(false);
                player2SelectingVisual.SetActive(false);
            }
        }
    }
}
