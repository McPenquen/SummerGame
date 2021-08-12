using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    // List of all in game objects marking locations where the instructions will display at
    [SerializeField] GameObject[] instructionsObjects;
    // List of all UI instructions to be displayed at the saved locations - they are linked by common index
    [SerializeField] GameObject[] uiInstructionsObjects;
    // Players
    [SerializeField] PlayersManager players;
    // The index of the last displayed instruction
    private int currentInstructionId = 0;
    // Bool if the instructions is showing or not yet
    private bool isShowingCurrentInstruction = false;
    // Current instruction countdown
    private float instructionDisplayCountdown = 10.0f;

    void Start()
    {
        // All insturctions should remain hidden until they are reached by the players
        foreach (GameObject instruction in uiInstructionsObjects)
        {
            instruction.SetActive(false);
        }
    }

    void Update()
    {
        // If we've displayed all instructions, skip
        if (currentInstructionId < instructionsObjects.Length)
        {
            // if the players have reached the instruction and it is not yet shown trigger showing it
            if (players.GetPosition().x >= instructionsObjects[currentInstructionId].transform.position.x && !isShowingCurrentInstruction)
            {
                isShowingCurrentInstruction = true;
                uiInstructionsObjects[currentInstructionId].SetActive(true);
            }
            // if the instructions is displayed decrease the timer of the dt
            if (isShowingCurrentInstruction)
            {
                if (instructionDisplayCountdown > 0.0f)
                {
                    instructionDisplayCountdown -= Time.deltaTime;
                }
                // else hide the instruction
                else
                {
                    isShowingCurrentInstruction = false;
                    uiInstructionsObjects[currentInstructionId].SetActive(false);
                    instructionDisplayCountdown = 10.0f;
                    // Move onto next instruction
                    currentInstructionId++;
                }
            }
        }
    }
}
