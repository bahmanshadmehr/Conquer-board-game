using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;




    public class Continent
    {
        private int power_of_continent;
        private int continents_count;
        private ArrayList my_graph = new ArrayList();



        //This constructor will open a file and will read the data from it and will simuluate a graph with an ArrayList
        public Continent(String address, int soldiers)
        {
            power_of_continent = soldiers;
                string text = System.IO.File.ReadAllText(address);
                my_graph = new ArrayList();

                //Spliting the file and storing lines in an Array
                String[] substrings = text.Split('\r');

                continents_count = substrings.Length;

                //This line will write our graph in console
                //Console.Write(String.Join("", substrings));

                //This loop is supposed to use seperated lines and make an ArrayList of them and add it to my_graph ArrayList
                foreach (String line in substrings)
                {
                    //The line should be cleaned and after that it should be broken to the nodes 
                    //and nodes should be stored in an temporary arraylist
                    String cleaned_line = line.Replace("\n", string.Empty);

                    String[] nodes = cleaned_line.Split(' ');

                    ArrayList nodesArray = new ArrayList();

                    //This loop is supposed to make the temporary ArrayList which is going to be add to my_graph ArrayList
                    foreach (String node in nodes)
                    {
                        nodesArray.Add(node);
                    }

                    //Adding temporary ArrayList to our graph
                    my_graph.Add(nodesArray);
                }

                //If my file is not a good designed class, this Text will be shown

                if (!check_validity())
                { Console.WriteLine("Invalid File"); }

            
        }


        //check file design, the file design should be in this way:
        //1. It should not have an extra \n at the end
        //2. Spacing should be ok
        //3. Some files have \n and some other have \r
        //4. each line should be for a Continent
        //5. each country, should be onlyb in one line
        private Boolean check_validity() 
        {
            int row_number_of_outer_loop = 0;
            foreach (ArrayList array in my_graph)
            {
                //this first and second loops are for accessing the nodes in the line
                foreach (String node in array)
                {
                    int row_number_of_inner_loop = -1;

                    //This loop is for accessing array rows and checking that nodes wont be on other lines too
                    foreach (ArrayList inner_loop_array in my_graph)
                    {
                        row_number_of_inner_loop += 1;
                        if (row_number_of_inner_loop == row_number_of_outer_loop)
                        {
                            //if the line is same as the line that our node is from, we should skip it
                            continue;
                        }
                        else if (inner_loop_array.Contains(node))
                        {
                            //if our node is in another line,this method will retun false
                            return false;
                        }

                    }


                }

                row_number_of_outer_loop += 1;

            }
            //At last, if no nodes was repeated twice, this nethod will return true
            return true;
        }

        //This method will check the countries given to it and will return an ArrayList from all af the continents 
        //that all of their countries are in the first arraylist
        //And this will take an Arraylist of continents, and wont check for that Continents, 
        //beacuse we already know that they are all in the list
        public ArrayList check_continent(List<int> Countries_ID, ArrayList Continent_ID)
        {
            ArrayList c = new ArrayList();

            int contienent_id = 0;

            //This method will work with two loops, first loop will get all countries in a continent
            //Second loop will check if that countries are in the player given country ArrayList
            //If the flag is 1, it means that all of the countries exists in the given arrayList
            foreach (ArrayList array in my_graph)
            {
                int flag = 1;
                foreach (String node in array)
                {

                    if (!(Continent_ID.Contains(node)))
                    {
                        flag = 0;
                        break;
                    }
                }
                if (flag == 1)
                {
                    c.Add(contienent_id.ToString());
                }
                contienent_id += 1;
            } return c;
        }

        //This method is for getting all continent count
        public int get_continent_count()
        {
            return continents_count;
        }

        //Returning all countries in a continent
        public ArrayList get_a_continent_countries(int continent_id)
        {
            return my_graph[continent_id] as ArrayList;
        }



    }






