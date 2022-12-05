using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _setText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _tipText;

    [SerializeField] private Image[] icons;
    [SerializeField] private Sprite sadnessIcon;

    public void SetGameStatus(bool isWon, string status, string tip)
    {
        // Text
        _setText.text = isWon ? "Game Complete" : "Game Over";
        _statusText.text = status;
        
        if (string.IsNullOrEmpty(tip))
        {
            _tipText.gameObject.SetActive(false);
        }
        else
        {
            _tipText.text = tip;
        }

        // Icons
        if (!isWon)
        {
            foreach(Image icon in icons)
            {
                icon.sprite = sadnessIcon;
            }
        }
    }
}
