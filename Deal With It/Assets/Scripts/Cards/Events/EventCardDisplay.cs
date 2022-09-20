using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCardDisplay : MonoBehaviour
{
    public Event eventCard;

    public Text nameText;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = eventCard.CardName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
