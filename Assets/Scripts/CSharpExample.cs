using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpExample : MonoBehaviour
{
    // Public Variable
    public string variableOne = "Hello, I'm a public variable! I'm visible on the Unity Editor";

    // Private Variable
    private string m_variableTwo = "Hello! I'm a private variable! I'm not visible on the Unity Editor!";

    // Serialized Private Variable
    [SerializeField] private string m_variableThree = "Hello! I'm also a private variable, but I have been serialized so I can be seen in the Unity Editor!";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
