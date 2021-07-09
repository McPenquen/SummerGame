using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallax : MonoBehaviour
{
    // The width of the image and the initial position
    private float width, startPosition;
    [SerializeField] private GameObject camera;
    [SerializeField] private float parallexEffect;

    void Start()
    {
        startPosition = transform.position.x;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float parCamPos = camera.transform.position.x * (1 - parallexEffect);
        // Move the BG according to the parallexEffect value
        float distance = camera.transform.position.x *  parallexEffect;
        transform.position = new Vector3(startPosition - distance, transform.position.y, transform.position.z);
        // Repeat the BG texture when at the end
        if (parCamPos > startPosition + width)
        {
            startPosition += width;
        }
        else if (parCamPos < startPosition - width)
        {
            startPosition -= width;
        }
    
    }
    // parallax effect inspired from: https://www.youtube.com/watch?v=zit45k6CUMk&ab_channel=Dani
}
