using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    /* ---------------------------- For all tooltips ---------------------------- */
    [SerializeField] private CardType cardType;

    /* ------------------------ For Event + Action Cards ------------------------ */
    // * Does not affect NPC card
    [SerializeField] private LevelType _levelType;

    /* ------------------------------ Info Sources ------------------------------ */
    // NPC Card
    private NPC _npc;

    // Event Card
    private EventCardDisplay _eventCardInfo;

    // Action Card
    private Action _actionCardInfo;

    /* -------------------------------- Tooltips -------------------------------- */
    // NPC Card
    private TooltipTrigger _goalTooltip;
    private TooltipTrigger _eventTooltip;
    private TooltipTrigger _strategyTooltip;

    // Event + Action Cards
    private TooltipTrigger _traitTooltip;
    private TooltipTrigger _inOrderTooltip;
    private TooltipTrigger _atLeastTooltip;

    void Start()
    {
        if (cardType == CardType.NPC)
        {
            SetUpNPCTooltips();
        }

        else if (cardType == CardType.Event)
        {
            // Get info source
            _eventCardInfo = (EventCardDisplay)GameObject.FindGameObjectWithTag("Event Card Controller").GetComponent<EventCardDisplay>();

            InitializeEffectTooltips();
        }

        else if (cardType == CardType.Action)
        {
            InitializeEffectTooltips();
        }
    }

    void Update()
    {
        // Update tooltip contents
        if (cardType == CardType.Event)
        {
            WriteEventCardTooltips();
        }

        else if (cardType == CardType.Action)
        {
             // Get info source
            _actionCardInfo = this.transform.parent.gameObject.GetComponentInParent<ActionCardDisplay>().CurrentActionCard;

            WriteActionCardTooltips();
        }
    }

    private void SetUpNPCTooltips()
    {
        // Get info source
        _npc = GetComponentInParent<NPCDisplay>().npc;

        // Set NPC tooltips
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
        
        // Tooltips content
        if (_npc != null)
        {
            _goalTooltip.Content = _npc.CardGoals;
            _eventTooltip.Content = _npc.EventEffects;
            _strategyTooltip.Content = _npc.StrategyEffects;
        }
    }

    // Get tooltip triggers
    private void InitializeEffectTooltips()
    {
        TooltipTrigger[] tooltips = GetComponents<TooltipTrigger>();
        for (int i = 0; i < tooltips.Length; i++)
        {
            if (tooltips[i].Type == TooltipType.Trait)
            {
                _traitTooltip = tooltips[i];
            }
            else if (tooltips[i].Type == TooltipType.InOrderCombo)
            {
                _inOrderTooltip = tooltips[i];
            }
            else if (tooltips[i].Type == TooltipType.AtLeastCombo)
            {
                _atLeastTooltip = tooltips[i];
            }
        }
    }

    // Get tooltips content for event
    private void WriteEventCardTooltips()
    {
        if (_eventCardInfo != null)
        {
            // NPC Traits
            if (_levelType == LevelType.Energy)
            {
                _traitTooltip.Content = _eventCardInfo.EnergyTraitEffectText;
            }
            else if (_levelType == LevelType.Joy)
            {
                _traitTooltip.Content = _eventCardInfo.JoyTraitEffectText;
            }
            else if (_levelType == LevelType.Sadness)
            {
                _traitTooltip.Content = _eventCardInfo.SadnessTraitEffectText;
            }
            else if (_levelType == LevelType.Fear)
            {
                _traitTooltip.Content = _eventCardInfo.FearTraitEffectText;
            }
            else if (_levelType == LevelType.Anger)
            {
                _traitTooltip.Content = _eventCardInfo.AngerTraitEffectText;
            }
        }
    }

    // Get tooltips content for action cards
    private void WriteActionCardTooltips()
    {
        if (_actionCardInfo != null)
        {
            // NPC Traits
            if (_levelType == LevelType.Energy)
            {
                _traitTooltip.Content = _actionCardInfo.EnergyTraitEffectText;
            }
            else if (_levelType == LevelType.Joy)
            {
                _traitTooltip.Content = _actionCardInfo.JoyTraitEffectText;
            }
            else if (_levelType == LevelType.Sadness)
            {
                _traitTooltip.Content = _actionCardInfo.SadnessTraitEffectText;
            }
            else if (_levelType == LevelType.Fear)
            {
                _traitTooltip.Content = _actionCardInfo.FearTraitEffectText;
            }
            else if (_levelType == LevelType.Anger)
            {
                _traitTooltip.Content = _actionCardInfo.AngerTraitEffectText;
            }
        }
    }
}
