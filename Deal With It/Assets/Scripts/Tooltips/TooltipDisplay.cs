using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    /* ------------------------------ Info Sources ------------------------------ */
    private NPC npc;

    /* -------------------------------- Tooltips -------------------------------- */
    private TooltipTrigger _goalTooltip;
    private TooltipTrigger _eventTooltip;
    private TooltipTrigger _strategyTooltip;

    // Start is called before the first frame update
    void Start()
    {
        // Get sources
        npc = GetComponentInParent<NPCDisplay>().npc;

        // Set tooltips
        TooltipTrigger[] tooltips = GetComponents<TooltipTrigger>();
        for (int i = 0; i < tooltips.Length; i++)
        {
            if (tooltips[i].Type == TooltipType.NPCGoal)
            {
                _goalTooltip = tooltips[i];
            }
            else if (tooltips[i].Type == TooltipType.NPCEvent)
            {
                _eventTooltip = tooltips[i];
            }
            else if (tooltips[i].Type == TooltipType.NPCStrategy)
            {
                _strategyTooltip = tooltips[i];
            }
        }
        
        if (npc != null)
        {
            _goalTooltip.Content = npc.CardGoals;
            _eventTooltip.Content = npc.EventEffects;
            _strategyTooltip.Content = npc.StrategyEffects;
        }
    }
}
