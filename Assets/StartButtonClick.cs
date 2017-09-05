using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButtonClick : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the start button!");
        Application.LoadLevel("initilize_the_game");
    }
}