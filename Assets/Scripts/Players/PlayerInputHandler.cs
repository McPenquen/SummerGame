using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Player Input
    private PlayerInput m_playerInput;

    // Player Character
    private Character m_character;

    /*
     * AWAKE METHOD
     * 
     * Standard Unity method.
     * 
     * Method first obtains the player input component,
     * then obtains the character components in the 
     * scene.
     * 
     * Method then obtains the player index value
     * and assigns the character based on that value.
     * E.g. First player connected will control the
     * first character, etc.
     * 
     */
    private void Awake()
    {
        // Obtain the player input component
        m_playerInput = GetComponent<PlayerInput>();


        // Check if player input is set to one controller
        if(m_playerInput)
        {
            //something
        }
        // One Keyboard
        // Multiple Controller


        // Obtain the number of characters in the scene
        var characters = FindObjectsOfType<Character>();

        // Obtain the player index value
        var index = m_playerInput.playerIndex;

        // Assign the player index value to the character
        m_character = characters.FirstOrDefault(c => c.GetPlayerIndex() == index);
    }

    /*
     * ON MOVE METHOD
     * 
     * Method first checks the character
     * object exists.
     * 
     * If the character object exists,
     * the SetXValue method is invoked
     * based on the X value of the Vector2
     * input.
     */
    public void OnMove(InputAction.CallbackContext context)
    {
        // Check the character object exists
        if (m_character != null)
        {
            // Not null, set the X input value based on the X value read from the the input
            m_character.SetXValue(context.ReadValue<Vector2>().x);
        }
    }

    /*
     * ON CLIMB METHOD
     * 
     * Method first checks the character
     * object exists.
     * 
     * If the character object exists,
     * the SetYValue method is invoked
     * based on the Y value of the Vector2
     * input.
     */
    public void OnClimb(InputAction.CallbackContext context)
    {
        // Check the character object exists
        if (m_character != null)
        {
            // Not null, set the Y input value based on the Y value read from the the input
            m_character.SetYValue(context.ReadValue<Vector2>().y);
        }
    }

    /*
     * ON JUMP METHOD
     * 
     * Method first checks if the character
     * exists. If it does, the method then
     * checks that the context has just been
     * performed.
     * 
     * If the context has just been performed,
     * it then invoked the jump method on the 
     * character.
     */
    public void OnJump(InputAction.CallbackContext context)
    {
        // Check the character object exists
        if(m_character !=null)
        {
            // Check that the context has just been performed (button has been pressed)
            if(context.performed)
            {
                // Invoke the jump method for the player's character
                m_character.Jump();
            }
        }
    }

    /*
     * ON GRAB METHOD
     * 
     * Method first checks if the character
     * exists. If it does, the method then
     * checks that the context has just been
     * performed.
     * 
     * If the context has just been performed,
     * it then invoked the jump method on the 
     * character.
     */
    public void OnGrab(InputAction.CallbackContext context)
    {
        // Check that the character object exists
        if(m_character != null)
        {
            // Check if the context has been performed
            if(context.performed)
            {
                // Invoke grab method for the character
                m_character.Grab();
            }
            // Else context has stopped
            else
            {
                // Invoke the release method
                m_character.Release();
            }
        }
    }
}
