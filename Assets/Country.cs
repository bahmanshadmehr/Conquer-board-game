using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class Country
    {
        private int ID;
        private int player_ID;
        private int soldiers_count;
        private Map my_map;

        //The constructor needs to get a map from user and assign it to our my_map variable
        public Country(Map map, int id)
        {
            my_map = map;
            soldiers_count = 0;
            ID = id;
        }

        //Some GET and SET methods to change our fields in the class

        public void set_soldiers(int n)
        {
            soldiers_count = n;
        }

        public void set_player(int id)
        {
            player_ID = id;
        }

        public int get_player_id()
        {
            return player_ID;
        }

        public int get_country_id()
        {
            return ID;
        }

        public int get_power()
        {
            return soldiers_count;
        }

        //This method will check if the change that we are going to make is valid
        private Boolean valid_soldier_change(int soldiers)
        {
            if (soldiers_count - soldiers < 1)
            {
                //The soldier_count can never be less than 1
                //Debug.Log("soldier count = " + soldiers_count + " soldiers = " + soldiers );
                return false;
            }

            return true;
        }

        //This function will change the soldiers_count, but since there are some conditions, it will check for them
        //If the return value is less than or equals to 0 an error has occured
        public int attack(Country target, int soldiers)
        {

            if (!valid_soldier_change(soldiers))
            {
                //The soldier_count can never be less than 1
                return 0;
            }
            else if (target.get_player_id() == player_ID)
            {
                //If player want to attack his own country, -1 will be returned
                return -1;
            }

            else if (!my_map.check(ID, target.get_country_id()))
            {
                //If user try to attack a country which is not a neigber to his country, -2 will be returned
                return -2;
            }

            //If everything is ok, 1 will be returned
            return 1;
        }

        //This method is not really needed for now
        public int defence(int soldiers)
        {
            if (soldiers > 2)
            {
                return -1;
            }
            if (!valid_soldier_change(soldiers))
            {
                //The soldier_count can never be less than 1
                return 0;
            }

            return 1;
        }

        //This method will change soldiers from a country
        public void change_soldiers(int soldiers)
        {
            soldiers_count -= soldiers;
        }



    }

