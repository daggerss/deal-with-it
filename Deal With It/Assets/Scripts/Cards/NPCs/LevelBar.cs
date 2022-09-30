using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Slider SliderBar;

    /* ----------------------------- Custom Methods ----------------------------- */
    public void SetMaxValue(int value)
    {
        SliderBar.maxValue = value;
    }

    public void SetValue(int value)
    {
        SliderBar.value = value;
    }
}
