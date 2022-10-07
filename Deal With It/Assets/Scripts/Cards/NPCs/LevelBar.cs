using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Slider SliderBar;

    public float Value => SliderBar.value;

    /* ----------------------------- Custom Methods ----------------------------- */
    public void SetMaxValue(int value)
    {
        SliderBar.maxValue = value;
    }

    public void SetValue(float value)
    {
        SliderBar.value = value;
    }
}
