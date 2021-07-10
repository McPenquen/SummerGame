using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallax : MonoBehaviour
{
    // The width of the image and
    private float width;
    // Initial position of  both BG objects
    private float[] initPositions = {0, 0};
    // Children list
    [SerializeField] private GameObject[] bgChildren;
    // The child index of the active bg
    private int activeChildIndex;
    [SerializeField] private GameObject camera;
    // The speed of the BG movement, 4 - is similar movement to the camera, lower values increase x incrementing
    [SerializeField] private float parallexEffect;

    void Start()
    {
        Vector3 bg1Pos = bgChildren[0].transform.position;
        Vector3 bg2Pos = bgChildren[1].transform.position;

        Vector3 cam2DPos = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);

        float bg1Distance = (cam2DPos - bg1Pos).magnitude;
        float bg2Distance = (cam2DPos - bg2Pos).magnitude;

        // Save width of the bg sprite
        width = bgChildren[0].gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        // Set the activeIndex to the background in the front of the cam
        activeChildIndex = bg1Distance < bg2Distance ? 0 : 1;

        // Align the Bg child objects based on the cam postion in relation to the BG object's edge
        if (activeChildIndex == 0)
        {
            // Move it only if it's positioned wrong
            if (cam2DPos.x < bg1Pos.x && bg2Pos.x > bg1Pos.x) 
            {
                moveChild(1, -width);
            }
            else if (cam2DPos.x > bg1Pos.x && bg2Pos.x < bg1Pos.x)
            {
                moveChild(1, width);
            }
        }
        else
        {
            // Move it only if it's positioned wrong
            if (cam2DPos.x < bg2Pos.x && bg2Pos.x < bg1Pos.x) 
            {
                moveChild(0, -width);
            }
            else if (cam2DPos.x > bg2Pos.x && bg2Pos.x > bg1Pos.x)
            {
                moveChild(0, width);
            }
        }

        // Save initial positions
        initPositions[0] = bgChildren[0].transform.position.x;
        initPositions[1] = bgChildren[1].transform.position.x;
}

    void Update()
    {
        int notActiveChild = activeChildIndex == 0 ? 1 : 0;

        // Move the BG obejcts according to the parallexEffect value
        float distance = camera.transform.position.x / parallexEffect;
        for (int i = 0; i < bgChildren.Length; i++)
        {
            bgChildren[i].transform.position = new Vector3(
                initPositions[i] + distance, bgChildren[i].transform.position.y, bgChildren[i].transform.position.z);
        }        
        
        // Set the active bg child
        if (camera.transform.position.x < bgChildren[activeChildIndex].transform.position.x - width / 2 
            || camera.transform.position.x > bgChildren[activeChildIndex].transform.position.x + width / 2)
        {
            activeChildIndex = notActiveChild;
        }

        // Move the other bg child based on the distance to the edge
        if (bgChildren[notActiveChild].transform.position.x > bgChildren[activeChildIndex].transform.position.x
            && camera.transform.position.x >= (bgChildren[activeChildIndex].transform.position.x - 5.0f)
            && camera.transform.position.x < (bgChildren[activeChildIndex].transform.position.x)
        )
        {
            moveChild(notActiveChild, -width);
        }
        if (bgChildren[notActiveChild].transform.position.x < bgChildren[activeChildIndex].transform.position.x
            && camera.transform.position.x <= (bgChildren[activeChildIndex].transform.position.x + 5.0f)
            && camera.transform.position.x > (bgChildren[activeChildIndex].transform.position.x)
        )
        {
            moveChild(notActiveChild, width);
        }
    }
    // parallax effect inspired from: https://www.youtube.com/watch?v=zit45k6CUMk&ab_channel=Dani

    // Move the child object with the index the selected amount along the x axis
    private void moveChild(int index, float distance)
    {
         bgChildren[index].transform.position = new Vector3(
            initPositions[index] + distance * 2, bgChildren[index].transform.position.y, bgChildren[index].transform.position.z
        );
        // Update the new x initial value
        initPositions[index] = bgChildren[index].transform.position.x;
    }
}
