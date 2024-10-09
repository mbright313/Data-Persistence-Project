using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    public static NameScript Instance;
    public TMP_InputField nameInput;
    public Button StartButton;
    private string playerName;
    private void Awake()
    {
        if(Instance != null)//if when going back to the main menu there is another instance of the namekeeper, destroy the newly created keeper
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartButtonPress()
    {
        if(nameInput.text != null)
        {
            playerName = nameInput.text;
            SceneManager.LoadScene(1);
        }
    }

    public string getName()
    {
        return playerName;
    }
}
