using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    // Counter of update method
    private int updateCounter = 0;
    // Selected Child index
    private int selectedChildIndex = 0;
    // Buttons list
    [SerializeField] private GameObject[] buttons;

    void Update()
    {
        if (updateCounter <= 1)
        {
            // The first time around it clears the object from event system
            if (updateCounter == 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            // The second time around it selects a new default object
            else if (updateCounter == 1)
            {
                // Automatically select the first child of the menu
                EventSystem.current.SetSelectedGameObject(buttons[selectedChildIndex]);
            }
            updateCounter++;
        }
        // Navigate up and down in the menu with keys
        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedChildIndex != 0)
        {
            selectedChildIndex--;
            updateCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && selectedChildIndex != buttons.Length - 1)
        {
            selectedChildIndex++;
            updateCounter = 0;
        }
    }
    private void OnDisable() 
    {
        // On disable we resetthe update counter
        updateCounter = 0;
        selectedChildIndex = 0;
    }
}
