using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    private Animator animator;
    static bool isWon = false;
    // Players
    [SerializeField] private GameObject players;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // Start taking off animation
        if (isWon)
        {
            players.SetActive(false);
            animator.SetBool("isTakingOff", true);
        }
    }

    public static void SetIsWon(bool b)
    {
        isWon = b;
    }
}
