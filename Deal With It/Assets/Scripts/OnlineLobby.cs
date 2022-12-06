using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OnlineLobby : MonoBehaviour
{
    /* --------------------------------- Buttons -------------------------------- */
    public Button CreateRoomButton;
    public Button JoinRoomButton;
    public Button RegisterButton;

    /* ---------------------------------- Text ---------------------------------- */
    public TMP_Text Title;
    public TMP_Text SecondaryText;

    /* ------------------------------- Input Field ------------------------------ */
    public TMP_InputField NameField;

    /* ------------------------- Join Room / Create Room ------------------------ */
    private string _lobbyAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomButtonPressed(string lobbyAction){
        if(lobbyAction == "Create"){
            SecondaryText.text = "Create Room";
            _lobbyAction = lobbyAction;
        }else if(lobbyAction == "Join"){
            SecondaryText.text = "Join Room";
            _lobbyAction = lobbyAction;
        }else{
            // Nothing
        }

        CreateRoomButton.gameObject.SetActive(false);
        JoinRoomButton.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);

        SecondaryText.gameObject.SetActive(true);
        NameField.gameObject.SetActive(true);
        RegisterButton.gameObject.SetActive(true);
    }

    public void Register(){
        SceneManager.LoadScene(1);
    }
}
