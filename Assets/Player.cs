using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    class Player
    {
        private static readonly System.Random t = new System.Random();
        
        public static int player_count = 0;

        private int player_id;
        private String name;
        private List<int> countries = new List<int>();
        private ArrayList continents = new ArrayList();
        private int player_soldiers = 0;
        private int free_soldiers = 0;
        private int card_count = 15;
        private Bald bald;
        private Map map;

        public Player(Map main_map, String _name)
        {
            //Player id will be equal to player count
            player_id = player_count;
            player_count += 1;
            map = main_map;
            //set_countries_and_continents();
            name = _name;
            bald = new Bald();
        }

        //This method will get first randomly given countries to the player
        //and will chec if a continent is given to him too
        public void set_countries_and_continents()
        {
            Debug.Log("player_id = " + player_id);

            countries = map.get_player_countries(player_id)[player_id];
            continents = map.check_player_continents(countries, continents);
        }


        //This method will return soldiers given to the player in 'taqviat' part
        public int get_free_soldiers()
        {
            return free_soldiers;
        }

        //This method is for adding some soldiers to free soldiers
        public void add_free_soldiers(int n)
        {
            free_soldiers += n;
        }

        //Return name of the player
        public String get_name()
        {
            return name;
        }

        //Return player id
        public int get_id()
        {
            return player_id;
        }

        //Returns cards count
        public int get_cards()
        {
            return card_count;
        }

        //Add some cards
        public void add_cards(int n)
        {
            card_count += n;
        }

        //Return country count, the number of countries that the player has
        public int get_countries_count()
        {
            return countries.Count;
        }


        //The attack class will need a defencer player, and a from country and a to country
        public int attack(Country from_country, Country to_country, Map my_map, int bald_count)
        {

            int sol_count = 3;

            //this line will check if our attack is valid
            int attack_status = from_country.attack(to_country,bald_count);

            if (attack_status == 0){
                sol_count = from_country.get_power()-1;
            }
            else if (attack_status == -1)
            {
                to_country.change_soldiers(0 - bald_count);
                from_country.change_soldiers(bald_count);
                return attack_status;
            }

            if (attack_status != 1){
                //If attack is not valid, status will be return for more handeling
                return attack_status;
            }

            //Two arrays for storing bald result
            //Length should be the minimum of the given numbers in code
            int[] soldier_attack_array = bald.bald(Math.Min(3, bald_count));
            int[] soldier_defence_array = bald.bald(Math.Min(2, to_country.get_power()));

            //int attacker_winner_soldiers = 0;
            int attacker_lose_soldiers = 0;

            //This loop will check how many soldiers have died in the battle
            for (int i = 0; i < Math.Min(Math.Min(2, to_country.get_power()) , bald_count); i++)
            {
                if (soldier_attack_array[i] < soldier_defence_array[i])
                {
                    attacker_lose_soldiers += 1;
                }
            }

            //This lines will change soldiers in two countries :
            //Attacker and defencer
            to_country.change_soldiers(Math.Min(Math.Min(2, to_country.get_power()) , bald_count - attacker_lose_soldiers));
            from_country.change_soldiers(attacker_lose_soldiers);

            //Check if the attacker has wone, if so, it will returns 10
            if (to_country.get_power() <= 0)
            {
                to_country.set_soldiers(0);
                return 10;
            }

            //if attack was completed without winning, 1 will be return
            return 1;
        }

        //this method will add some soldiers to the player soldirs, it will be used 
        public void change_soldiers_by(int soldiers)
        {
            player_soldiers += soldiers;
        }

        //This method will give free soldiers to player
        //It will give him 1/3 of country count + the count that programmer has entered
        public void set_soldiers_by_countries(int n)
        {
            int i = 0;
            if ((countries.Count / 3) > 3)
            {
                i = countries.Count / 3;
            }
            else i = 3;

            free_soldiers += n + i;
        }

        //This will turn cards to free soldiers
        public void change_card(int n)
        {
            if (card_count - n < 0) { return; }
            free_soldiers += n * 5;
            card_count -= n;
        }

        //This method will get a generic list and will assign it to player's country list
        public void set_countries(List<int> _countries)
        {
            countries = _countries;
        }

        //For the first time that the map will be loaded, some soldiers will be given to player, and they will be randomly seperated in his countries
        //This method works like this:
        //It will create a list of integers for storing all of player's countrie's soldiers
        //it will randomly fill the list with numbers between 1 and 3
        //Then it will cut the nodes with 3 and 2 and 1 only by one
        //For example if the difference is more than 3 it will find a home with number 3, and it will make it 2
        //then if its still more than 2 it will find a home with 2 and cut it by one
        //and so on...
        //untill the sum of array is less than free soldiers
        //and finally it will assign 0 to free soldiers
        public List<int> seperate_soldiers_randomly(List<int> _countries)
        {
            List<int> soldiers_for_countries = new List<int>();
            int total_sum = 0;

            for (int i = 0; i < _countries.Count; i++)
            {
                soldiers_for_countries.Add(t.Next(1, 4));
            }

            total_sum = sum_of_array(soldiers_for_countries);


            while (total_sum > free_soldiers)
            {
                if (total_sum - free_soldiers > 3)
                {
                    for (int i = 0; i < soldiers_for_countries.Count; i++)
                    {
                        if (soldiers_for_countries[i] == 3)
                        {
                            soldiers_for_countries[i] = 2;
                            break;
                        }
                    }
                }


                if (total_sum - free_soldiers > 1)
                {
                    for (int i = 0; i < soldiers_for_countries.Count; i++)
                    {
                        if (soldiers_for_countries[i] == 2)
                        {
                            soldiers_for_countries[i] = 1;
                            break;
                        }
                    }
                }


                if (total_sum - free_soldiers > 0)
                {
                    for (int i = 0; i < soldiers_for_countries.Count; i++)
                    {
                        if (soldiers_for_countries[i] == 1)
                        {
                            soldiers_for_countries[i] = 0;
                            break;
                        }
                    }
                }

                total_sum = sum_of_array(soldiers_for_countries);

            }

            free_soldiers = 0;

            return soldiers_for_countries;
        }

        //This method will get an array and will return all of it nodes
        private int sum_of_array(List<int> int_array)
        {
            int sum = 0;

            foreach (int i in int_array)
            {
                sum += i;
            }

            return sum;
        }
      
    }



