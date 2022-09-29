using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    private int _Round = 0;
    public int Round
    {
        get
        {
            return _Round;
        }
        set
        {
            _Round = value;
        }
    }

    // Executes on next round
    public void NextRound(){
        _Round++;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
