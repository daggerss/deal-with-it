using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    [SerializeField] private Tooltip generalTooltip;
    [SerializeField] private Tooltip goalTooltip;
    [SerializeField] private Tooltip eventTooltip;
    [SerializeField] private Tooltip strategyTooltip;
    [SerializeField] private Tooltip traitTooltip;
    [SerializeField] private Tooltip inOrderTooltip;
    [SerializeField] private Tooltip atLeastTooltip;
    
    private void Awake()
    {
        current = this;
    }

    public static void Show(TooltipType tooltipType, string content, string h1 = "", string prevStrat = "", string nextStrat = "")
    {
        // Only show if has content
        if (!string.IsNullOrEmpty(content))
        {
            // General tooltip
            if (tooltipType == TooltipType.General)
            {
                current.generalTooltip.SetText(content, h1);
                current.generalTooltip.gameObject.SetActive(true);
            }
            // [NPC] Goal tooltip
            else if (tooltipType == TooltipType.NPCGoal)
            {
                current.goalTooltip.SetText(content, h1);
                current.goalTooltip.gameObject.SetActive(true);
            }
            // [NPC] Event tooltip
            else if (tooltipType == TooltipType.NPCEvent)
            {
                current.eventTooltip.SetText(content, h1);
                current.eventTooltip.gameObject.SetActive(true);
            }
            // [NPC] Strategy tooltip
            else if (tooltipType == TooltipType.NPCStrategy)
            {
                current.strategyTooltip.SetText(content, h1);
                current.strategyTooltip.gameObject.SetActive(true);
            }
            // [Action] Trait tooltip
            else if (tooltipType == TooltipType.Trait)
            {
                current.traitTooltip.SetText(content, h1);
                current.traitTooltip.gameObject.SetActive(true);
            }
            // [Action] In order tooltip
            else if (tooltipType == TooltipType.InOrderCombo)
            {
                current.inOrderTooltip.SetText(content, h1, prevStrat, nextStrat);
                current.inOrderTooltip.gameObject.SetActive(true);
            }
            // [Action] At least tooltip
            else if (tooltipType == TooltipType.AtLeastCombo)
            {
                current.atLeastTooltip.SetText(content, h1, prevStrat, nextStrat);
                current.atLeastTooltip.gameObject.SetActive(true);
            }
        }
    }

    public static void Hide()
    {
        if (current.generalTooltip != null)
        {
            current.generalTooltip.gameObject.SetActive(false);
        }

        if (current.goalTooltip != null)
        {
            current.goalTooltip.gameObject.SetActive(false);
        }

        if (current.eventTooltip != null)
        {
            current.eventTooltip.gameObject.SetActive(false);
        }

        if (current.strategyTooltip != null)
        {
            current.strategyTooltip.gameObject.SetActive(false);
        }
        
        if (current.traitTooltip != null)
        {
            current.traitTooltip.gameObject.SetActive(false);
        }
        
        if (current.inOrderTooltip != null)
        {
            current.inOrderTooltip.gameObject.SetActive(false);
        }
        
        if (current.atLeastTooltip != null)
        {
            current.atLeastTooltip.gameObject.SetActive(false);
        }
    }
}
