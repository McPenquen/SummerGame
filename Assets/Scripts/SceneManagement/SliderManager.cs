using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI sliderText;
    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("0");
        });
    }
    protected void Update()
    {
        DetectArrows();
    }
    // Detect the left and right arrows to increase and decrease values (not always working)
    protected void DetectArrows()
    {
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
