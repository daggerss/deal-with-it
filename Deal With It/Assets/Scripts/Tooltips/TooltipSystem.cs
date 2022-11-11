using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;
    
    private void Awake()
    {
        current = this;
    }

    public static void Show(string content, string h1 = "", string prevStrat = "", string nextStrat = "")
    {
        current.tooltip.SetText(content, h1, prevStrat, nextStrat);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
