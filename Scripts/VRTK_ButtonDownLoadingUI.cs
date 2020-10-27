using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRTK_ButtonDownLoadingUI : MonoBehaviour
{
    public VRTK_ButtonDownLoading loading;
    public Slider slider; 
    // Start is called before the first frame update
    void Start()
    {
        ResetSliderValue();
    }

    private void ResetSliderValue()
    {
        slider.maxValue = 100;
        slider.minValue = 0;
        slider.value = 0;
    }

    public void UpdateSlider()
    {
        slider.value = loading.GetCurrentPercentage();
    }
}
