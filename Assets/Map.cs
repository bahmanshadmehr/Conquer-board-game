using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public class Map
    {
        private Graph my_graph;
        private Continent continent;
        public List<List<int>> countries_given_to_players;
        private int country_count = 0;

        //This constructor will make a graph object and opens the file and reads data from text file and store in in object
        //and here it will assign a Continent object to our local continent object
        public Map(string address, Continent main_continent)
        { 
            my_graph = new Graph(address);
            continent = main_continent;
            country_count = my_graph.get_total_nodes_count();
        }

        //check neighborhood, This method will return true if two given countries are neighbor
        public bool check(int first_node, int second_node)
        { 
            return my_graph.check(first_node, second_node);
        }

        //This method will assign countries to the players randomly,
        //in fact it will give each country to a random player
        public List<List<int>> give_countries_to_players(int player_count, int _country_count)
        {
            List<List<int>> countries = new List<List<int>>();

            //This loop will make an empty two demontional ArrayList, so we can use it for country assignment,
            //second demontion length most be as long as player_count
            for (int i = 0; i < player_count; i++)
            {
                countries.Add(new List<int>(100));
            }

            //This loop will assign countries to random players
            for (int j = 0; j < country_count; j++)
            {
                int player = UnityEngine.Random.Range(0, player_count); //rnd.Next(0, 3);
                countries[player].Add(j) ;
            }


            countries_given_to_players = countries;

            return countries;
        }


        //If we need to get a player countries, we can use this function
        public List<List<int>> get_player_countries(int player_id)
        {
            return countries_given_to_players;
        }

        //This method will return every continent that a player has incounterd
        public ArrayList check_player_continents(List<int> Countries_ID, ArrayList Continent_ID)
        {
            return continent.check_continent(Countries_ID, Continent_ID);
        }

        //This method is for getting total country counts
        public int get_total_country_count()
        {
            return country_count;
        }

        //When a player wins a country, using this method the new countries will be assign to map
        public void set_countries_array(List<List<int>> _countries)
        {
            countries_given_to_players = new List<List<int>>();
            countries_given_to_players = _countries;
        }

    }
