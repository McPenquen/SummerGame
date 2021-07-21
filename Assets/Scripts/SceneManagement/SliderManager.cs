using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("0");
        });
    }
    private void Update()
    {
        // Read the left and right arrows to increase and decrease values 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slider.value -= 10;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            slider.value += 10;
        }
    }
}
