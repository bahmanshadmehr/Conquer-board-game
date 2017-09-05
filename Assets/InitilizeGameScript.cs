using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitilizeGameScript : MonoBehaviour
{
    //This code is for getting players name and storing player objects in an arraylist
    private Map map;
    private Continent continent;

    private GameScript gameScript;
    public Button addButton;
    public Button startButton;

    public ArrayList player_arraylist;

    public InputField name_field;
    public Text players_list;


    void Start()
    {
        //With this line, tis class wont be destroyd after changing the scene
        DontDestroyOnLoad(gameObject);

        player_arraylist = new ArrayList();

        players_list.text = "";

        //Getting info from file's for creating new players
        continent = new Continent("Assets\\Scripts\\Graphs\\Continent.txt", 1);
        map = new Map("Assets\\Scripts\\Graphs\\Graph.txt", continent);

        //Adding some onclickListener to our buttons
        Button addBtn = addButton.GetComponent<Button>();
        addBtn.onClick.AddListener(TaskOnAddClick);

        Button startBtn = startButton.GetComponent<Button>();
        startBtn.onClick.AddListener(TaskOnStartClick);
    }

    //This method is for adding new players if their name is not in the list
    void TaskOnAddClick()
    {
        //Checking that the name is not in the list
        if (!players_list.text.Contains(name_field.text + " "))
        {
            //Making new players and adding it to list
            Player player = new Player(map, name_field.text);
            player_arraylist.Add(player);
            players_list.text = players_list.text + "*" + " - " + name_field.text + " \n";
            name_field.text = "";
        }
    }


    void TaskOnStartClick()
    {
        //This line will load the main scene
        Application.LoadLevel("Main");
    }



}
