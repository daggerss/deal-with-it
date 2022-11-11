using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string heading;
    [SerializeField] private string prevStrat;
    [SerializeField] private string nextStrat;

    [SerializeField] [TextArea(1,5)] private string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, heading, prevStrat, nextStrat);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}
