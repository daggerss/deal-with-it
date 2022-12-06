using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private RoundController roundController;
    [SerializeField] private PlayerController player;

    [SerializeField] private Image _timerFill;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing the RoundController
        roundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Hide timer
        _timerFill.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!roundController.StopTimer && roundController.PlayerTurn == player.PlayerNumber){
            _timerFill.fillAmount = roundController.PlayerTimer / 30;
        }

        if(roundController.PlayerTurn != player.PlayerNumber)
        {
            _timerFill.fillAmount = 0f;
        }
    }
}
