using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject selectionSprite = null;
    [SerializeField] private bool hasSelectionImages = false;
    void Update()
    {
        if (hasSelectionImages)
        {
            // Show the selection sprite when it's selected
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                selectionSprite.SetActive(true);
            }
            else
            {
                selectionSprite.SetActive(false);
            }
        }
    }
}
