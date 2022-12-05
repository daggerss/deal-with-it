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

    public static void Hide(TooltipType type)
    {
        if (current.generalTooltip != null && type == TooltipType.General)
        {
            current.generalTooltip.gameObject.SetActive(false);
        }

        else if (current.goalTooltip != null && type == TooltipType.NPCGoal)
        {
            current.goalTooltip.gameObject.SetActive(false);
        }

        else if (current.eventTooltip != null && type == TooltipType.NPCEvent)
        {
            current.eventTooltip.gameObject.SetActive(false);
        }

        else if (current.strategyTooltip != null && type == TooltipType.NPCStrategy)
        {
            current.strategyTooltip.gameObject.SetActive(false);
        }
        
        else if (current.traitTooltip != null && type == TooltipType.Trait)
        {
            current.traitTooltip.gameObject.SetActive(false);
        }
        
        else if (current.inOrderTooltip != null && type == TooltipType.InOrderCombo)
        {
            current.inOrderTooltip.gameObject.SetActive(false);
        }
        
        else if (current.atLeastTooltip != null && type == TooltipType.AtLeastCombo)
        {
            current.atLeastTooltip.gameObject.SetActive(false);
        }
    }
}
