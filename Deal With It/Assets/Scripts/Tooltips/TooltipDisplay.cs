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
            // Strategy Order
            _inOrderTooltip.PrevStrat = _actionCardInfo.PrevIOStrategyText;
            _inOrderTooltip.NextStrat = _actionCardInfo.NextIOStrategyText;
            _atLeastTooltip.PrevStrat = _actionCardInfo.ALStrategyText;
            _atLeastTooltip.NextStrat = _actionCardInfo.ALStrategyText;

            // Energy
            if (_levelType == LevelType.Energy)
            {
                // NPC Traits
                _traitTooltip.Content = _actionCardInfo.EnergyTraitEffectText;

                // In Order Combos
                _inOrderTooltip.Content = _actionCardInfo.EnergyInOrderEffectText;

                // At Least Combos
                _atLeastTooltip.Content = _actionCardInfo.EnergyAtLeastEffectText;
            }
            // Joy
            else if (_levelType == LevelType.Joy)
            {
                // NPC Traits
                _traitTooltip.Content = _actionCardInfo.JoyTraitEffectText;

                // In Order Combos
                _inOrderTooltip.Content = _actionCardInfo.JoyInOrderEffectText;

                // At Least Combos
                _atLeastTooltip.Content = _actionCardInfo.JoyAtLeastEffectText;
            }
            // Sadness
            else if (_levelType == LevelType.Sadness)
            {
                // NPC Traits
                _traitTooltip.Content = _actionCardInfo.SadnessTraitEffectText;

                // In Order Combos
                _inOrderTooltip.Content = _actionCardInfo.SadnessInOrderEffectText;

                // At Least Combos
                _atLeastTooltip.Content = _actionCardInfo.SadnessAtLeastEffectText;
            }
            // Fear
            else if (_levelType == LevelType.Fear)
            {
                // NPC Traits
                _traitTooltip.Content = _actionCardInfo.FearTraitEffectText;

                // In Order Combos
                _inOrderTooltip.Content = _actionCardInfo.FearInOrderEffectText;

                // At Least Combos
                _atLeastTooltip.Content = _actionCardInfo.FearAtLeastEffectText;
            }
            // Anger
            else if (_levelType == LevelType.Anger)
            {
                // NPC Traits
                _traitTooltip.Content = _actionCardInfo.AngerTraitEffectText;

                // In Order Combos
                _inOrderTooltip.Content = _actionCardInfo.AngerInOrderEffectText;

                // At Least Combos
                _atLeastTooltip.Content = _actionCardInfo.AngerAtLeastEffectText;
            }
        }
    }
}
