using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

    public class Graph
    {
        private int total_node_count = 0;
        private ArrayList my_graph = new ArrayList();



        //This constructor will open a file and will read the data from it and will simuluate a graph with an ArrayList
        public Graph(String address)
        {
            string text = System.IO.File.ReadAllText(address);

            my_graph = new ArrayList();

            String[] substrings = text.Split('\r');

            Console.Write(String.Join("", substrings));

            //This loop is supposed to use seperated lines and make an ArrayList of them and add it to my_graph ArrayList
            foreach (String line in substrings)
            {
                String cleaned_line = line.Replace("\n", string.Empty);

                String[] nodes = cleaned_line.Split(' ');

                ArrayList nodesArray = new ArrayList();

                //This loop is supposed to make the temporary ArrayList which is going to be add to my_graph ArrayList
                foreach (String node in nodes)
                {
                    nodesArray.Add(node);
                }

                my_graph.Add(nodesArray);
                total_node_count += 1;
            }

            //If my file is not a good designed class, this exception will be raised
            if (!check_graph_validity())
            {
                throw new System.ArgumentException("Invalid File, Correct Your Graph");
            }


        }

        //This function will check the validability of my graph file, if it's not valid, it will raise an exception, if its valid, it will return true
        private Boolean check_graph_validity()
        {
            //In this function, if there is a node in a line, the line number should exists in the line with number of that node
            int counter = 0;
            foreach (ArrayList array in my_graph)
            {
                foreach (String node in array)
                {
                    int position = Int32.Parse(node);

                    ArrayList row = my_graph[position] as ArrayList;

                    if (!row.Contains(counter.ToString()))
                    {
                        throw new System.ArgumentException("counter = " + counter + "position = " + position);
                        //return false;    //This line is commented beacuse when an exception exists, that line would be unreachable
                        //But if the exeption was deleted, we need this line
                    }
                }
                counter += 1;
            }
            return true;
        }

        //This function will check if the two given nodes are neighbor or not
        public Boolean check(int first_node, int second_node)
        {
            //If two given nodes are the same an exception will be rise
            if (first_node == second_node)
            {
                throw new System.ArgumentException("Two given nodes are the same");
            }

            ArrayList row = my_graph[first_node] as ArrayList;

            if (!row.Contains(second_node.ToString()))
            {
                //Returns false if they are not neigbor
                return false;
            }
            //Returns true if they are neigbor
            return true;
        }

        //Returns the whole graph
        public ArrayList get_total_graph()
        {
            return my_graph;
        }

        //Returns all countries count
        public int get_total_nodes_count()
        { 
            return total_node_count;
        }


    }
