using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    private Animator animator;
    static bool isWon = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        isWon = false;
    }
    void Update()
    {
        // Start taking off animation
        if (isWon)
        {
            animator.SetBool("isTakingOff", true);
        }
    }

    public static void SetIsWon(bool b)
    {
        isWon = b;
    }
}
