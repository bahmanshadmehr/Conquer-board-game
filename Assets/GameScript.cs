using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public Camera cam;

    public int player_count = 0;
    private Map map;
    private Continent continent;
    private Bald bald;


    public Button attack_movement_button;
    public Button cancel_attack_button;

    private Button attack_movement_btn;

    private ContinentCeator continent_creator_script;

    //0: First Stage, giving countries to players
    //1: Reinforcement
    //2: Attack And Soldier Movement
    //3: Attacking And Moving
    //4: Game End
    //5: Card Change
    private int state = 0;
    private String[] state_list = { "Starting the game...", "Reinforcement", "Attack And Soldier exchange", "Defence", "End Game", "Card Changing" };
    private int current_player = 0;
    private int current_country = -1;
    private int attacked_country_id = -1;
    private ArrayList players_array;

    public ArrayList countries_coordinates = new ArrayList();
    public Text country_info;
    public Text current_player_info;
    public Text hovered_country_info;
    public InputField soldiers_field;
    public Button main_attack_button;

    List<List<int>> countries_given_to_players;


    void Start()
    {
        //Reading data from graph files and making a new map and an object for storing continent info
        continent = new Continent("Assets\\Continent.txt", 1);
        map = new Map("Assets\\Graph.txt", continent);

        //Getting players array from the object from previos scene, the array is in InitilizeGameScript class
        players_array = (GameObject.Find("initilize_game_ground").GetComponent("InitilizeGameScript") as InitilizeGameScript).player_arraylist;

        //after randomly giving countries to players, it will be assigned to our list
        countries_given_to_players = map.give_countries_to_players(players_array.Count, map.get_total_country_count());

        //This method wil initilize nedded infoes for starting the game
        make_the_game_ready();

        //Creating some onClickListener for some buttons, every buttons intraction will be explained
        attack_movement_btn = attack_movement_button.GetComponent<Button>();
        attack_movement_btn.onClick.AddListener(attack_movement_btn_click);

        Button cancel_operation_btn = cancel_attack_button.GetComponent<Button>();
        cancel_operation_btn.onClick.AddListener(cancel_btn_click);

        Button main_attack_btn = main_attack_button.GetComponent<Button>();
        main_attack_btn.onClick.AddListener(attack_on_click);

    }

    void Update()
    {
        //In every frame, this loop will check some of game infoes, like current player and state and more, and it will print it in the screen
        //If the game has ended, a propriete messafe will be showen
        foreach (Player p in players_array)
        {
            if (p.get_id() == current_player && state != 4)
            {
                current_player_info.text = p.get_name() + " - Cards = " + p.get_cards() + "\n";
                current_player_info.text += state_list[state] + "\n";

                current_player_info.text += "Free soldiers = " + p.get_free_soldiers();
            }
            else if (state == 4)
            {
                current_player_info.text = "Game Ended, \nCurrent Player have wone";
            }
        }
        
        //In every frame, it will check if we havve clicked or hover an object, and it will react in the best way
        ray_cast_handler();
    }

    //this method will initilize the game
    private void make_the_game_ready()
    {
        //this loop will be executed as player count, beacuse it needs to do some stuff for each player
        for (int i = 0; i < players_array.Count; i++)
        {
            //This line will set player countries and will take it from map object
            (players_array[i] as Player).set_countries(map.get_player_countries(i)[i]);

            //This lines will give some soldiers to player for start
            (players_array[i] as Player).set_soldiers_by_countries(0);
            (players_array[i] as Player).set_soldiers_by_countries(0);
            (players_array[i] as Player).set_soldiers_by_countries(0);

            //This line will seperate soldiers randomly, and will return a generic list of all of the players countrie's soldiers
            List<int> soldiers_for_countries = (players_array[i] as Player).seperate_soldiers_randomly(map.get_player_countries(i)[i]);

            //This will give some free soldiers for player to start with,
            (players_array[i] as Player).add_free_soldiers(5);

            //For each country given to the player that our main loop is iterating, this loop will find the country prephab that is instantiated
            //and will create its country object, and will initilize it
            //finding the object will be done by searching the name
            for (int j = 0; j < countries_given_to_players[i].Count; j++)
            {
                string country_name = "Country ";
                country_name += map.get_player_countries(i)[i][j].ToString();

                (GameObject.Find(country_name).GetComponent("CountryHandler") as CountryHandler).create_country(map, Int32.Parse(map.get_player_countries(i)[i][j].ToString()));
                (GameObject.Find(country_name).GetComponent("CountryHandler") as CountryHandler).country.set_player(i);
                (GameObject.Find(country_name).GetComponent("CountryHandler") as CountryHandler).country.set_soldiers(soldiers_for_countries[j]);
            }
        }

        //in start, if the player has some cards, the state will be 5 so he can turn them
        if ((players_array[current_player] as Player).get_cards() > 0) 
        { 
            state = 5;
            country_info.text = "Change Your Cards!";
        }
        //if not, state will be 1
        else { state = 1; }

    }

    //this method will handle mouse hover and mouse click
    public void ray_cast_handler()
    {
        //this objects is for handling  mouse hover and mouse click
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //If we have hover an object, in general and in every state
        if (Physics.Raycast(ray, out hit))
        {
            //If our object is a country
            if (hit.transform.gameObject.name.StartsWith("Country "))
            {
                String country_player = "";

                //Finding who the country belongs to
                foreach (Player p in players_array)
                {
                    if (p.get_id() == hit.transform.gameObject.GetComponent<CountryHandler>().country.get_player_id())
                    {
                        country_player = "Belongs to: " + p.get_name() + "   ";
                    }
                }

                //Getting country power
                String country_power = "Country power: " + hit.transform.gameObject.GetComponent<CountryHandler>().country.get_power().ToString();

                //Writing the info in the info panel
                hovered_country_info.text = country_player + country_power;
            }
        }

        //In state 1 or 2, when you click on a country, the current_country will be changed and the country info will be written at the top
        if (state == 2 || state == 1)
        {
            //This raycast is only for theclick, we dont need this to happen when the mouse hovers the object
            if (Input.GetMouseButtonDown(0))
            {
                //Checking if we have clicked something
                if (Physics.Raycast(ray, out hit))
                {
                    //Checking if it is a country
                    if (hit.transform.gameObject.name.StartsWith("Country "))
                    {
                        //Getting the information and writing it to panel
                        current_country = hit.transform.gameObject.GetComponent<CountryHandler>().country.get_country_id();
                        String country_name = "Country id: " + current_country.ToString() + "\n";
                        String country_player = "";
                        foreach (Player p in players_array)
                        {
                            if (p.get_id() == hit.transform.gameObject.GetComponent<CountryHandler>().country.get_player_id())
                            {
                                country_player = "Belongs to: " + p.get_name() + "\n";
                            }
                        }
                        String country_power = "Country power: " + hit.transform.gameObject.GetComponent<CountryHandler>().country.get_power().ToString() + "\n";
                        country_info.text = country_name + country_player + country_power;
                    }
                }
            }
        }

        //If state is 3, attacked country id will be changed so we can use it in our other method for attacking to it
        if (state == 3)
        {
            //This raycast is only for theclick, we dont need this to happen when the mouse hovers the object
            if (Input.GetMouseButtonDown(0))
            {
                //Checking if we have clicked something
                if (Physics.Raycast(ray, out hit))
                {
                    //Checking if it is a country
                    if (hit.transform.gameObject.name.StartsWith("Country "))
                    {
                        main_attack_button.enabled = true;
                        
                        //changing attack country
                        attacked_country_id = hit.transform.gameObject.GetComponent<CountryHandler>().country.get_country_id();
                        
                        //If the country is neigbor with the selected country, the word neigbor will be written to hover_info panel
                        if (map.check(attacked_country_id, current_country)) { hovered_country_info.text += " Neigbor"; }
                    }
                }
            }
        }

    }

    //when we want to submit a contry to current country for using it for attacking
    void attack_movement_btn_click()
    {
        //Writing the right message if the state is not 5
        if (state != 5)
        {
            if (GameObject.Find("Country " + current_country).GetComponent<CountryHandler>().country.get_player_id() != current_player)
            {
                hovered_country_info.text = "It's Not Your Turn.";
                return;
            }

            hovered_country_info.text = "Select A Country.";

            //Changing the state from attacking tio defence
            if (state == 2)
            {
                state = 3;
            }
        }

    }

    //This method will be changing states accordingly
    //And it will change the player in the right moment
    void cancel_btn_click()
    {
        if (state == 1)
        {
            state = 2;
            attack_movement_button.GetComponentInChildren<Text>().text = "Attack/Movement";
            cancel_attack_button.GetComponentInChildren<Text>().text = "Next Player";
        }

        else if (state == 2)
        {
            //Changing the player and going to next one
            if (current_player < players_array.Count - 1)
            {
                current_player += 1;
            }
            else
            {
                current_player = 0;

            }
            attack_movement_button.GetComponentInChildren<Text>().text = "Give Soldiers";
            cancel_attack_button.GetComponentInChildren<Text>().text = "Next Stage";
            state = 5;
            country_info.text = "Change Your Cards!";
        }
        else if (state == 3)
        {
            state = 2;
            cancel_attack_button.GetComponentInChildren<Text>().text = "Next Player";
        }
        else if (state == 5)
        {
            state = 1;
        }
    }

    //we have an attack button that will react according to the state that it currently is in
    void attack_on_click()
    {
        //if we are in state 5, it will be used to turn the cards of player, as musch as the player wants
        if (state == 5)
        {
            //The player can only keep three cards
            if ((players_array[current_player] as Player).get_cards() - Int32.Parse(soldiers_field.text) <= 3)
            {
                (players_array[current_player] as Player).change_card(Int32.Parse(soldiers_field.text));
            }
            else
            {
                hovered_country_info.text = "You can only keep 3 cards";
            }
        }

        //If state is 1, we can reinforcement the countries
        else if (state == 1)
        {
            //Finding the current country object
            Country country = GameObject.Find("Country " + current_country).GetComponent<CountryHandler>().country;
            //We can give soldires to a country that is not our own
            if(country.get_player_id() != current_player)
            {
                hovered_country_info.text = "Not your country.";
                return;
            }
            //We cant give soldiers more than we have
            else if ((players_array[current_player] as Player).get_free_soldiers() < Int32.Parse(soldiers_field.text))
            {
                hovered_country_info.text = "Not Enough Soldiers.";
                return;
            }
            
            //This will give the soldiers from player to country
            country.change_soldiers(0 - Int32.Parse(soldiers_field.text));
            (players_array[current_player] as Player).add_free_soldiers(0 - Int32.Parse(soldiers_field.text));
        }

        //If state is 2, It Only needs to change the text
        else if (state == 2)
        {
            cancel_attack_button.GetComponentInChildren<Text>().text = "Cancel Attack!";
        }

        //In this state, it will find the attacker and the defencer, so they can be used for attacking and defencing
        else if (state == 3)
        {
            //Finding the country objects
            Country attacker = GameObject.Find("Country " + current_country).GetComponent<CountryHandler>().country;
            Country defencer = GameObject.Find("Country " + attacked_country_id).GetComponent<CountryHandler>().country;

            //Checking if our attack is valid
            int attack_status = (players_array[attacker.get_player_id()] as Player).attack(attacker, defencer, map, Int32.Parse(soldiers_field.text));

            //Showing a message if our attack is not valid
            if (attack_status != 1 && attack_status != 10) { hovered_country_info.text = "Ivalid attack!"; }

            //if the attacker has won the country, this part will be executed
            if (attack_status == 10)
            {
                //adding the country to attacker country list and removing it from defencing country list
                countries_given_to_players[current_player].Add(defencer.get_country_id());
                countries_given_to_players[defencer.get_player_id()].Remove(defencer.get_country_id());

                //Give a card to player beacuse he has won a country
                (players_array[current_player] as Player).add_cards(1);

                //changing defencer country owner
                defencer.set_player(attacker.get_player_id());

                //submitting the new owners in the map
                map.set_countries_array(countries_given_to_players);

                //Checking if the game has ended
                if (countries_given_to_players[current_player].Count >= map.get_total_country_count())
                {
                    state = 4;
                    Debug.Log("You Won!");
                }
            }
        
        }

    }


}
