using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentCeator : MonoBehaviour {

    
    public float width;
    public float height;


    private CountryHandler country_script;



    public Continent my_continents = new Continent("Assets//Continent.txt", 20);
    public Map my_map;

    private int continent_count;
    private int country_count;

    private float[] continents_x_position;
    private float[] continents_y_position;

    private float[] countries_x_position;
    private float[] countries_y_position;

    public ArrayList continent_object_array = new ArrayList();
    public ArrayList country_object_array = new ArrayList();

    public GameObject continent;
    public GameObject country;





	// Use this for initialization
	void Start () {
        my_map = new Map("Assets/Graph.txt", my_continents);
        continent_creator();

        //Debug.Log(GameObject.Find("Game").GetComponent<GameScript>().countries_coordinates.Count);

        country_creator();
	}

    void Update()
    { 
        
    }



    void continent_creator()
    {
        continent_count = my_continents.get_continent_count();


        continents_x_position = new float[continent_count];
        continents_y_position = new float[continent_count];



        for (int i = 0; i < continent_count; i++)
        {
            bool flag = false;
            float xPosition = 0;
            float yPosition = 0;

            do
            {
                xPosition = UnityEngine.Random.Range(0f - width / 2f + 10f, width / 2f - 10f);
                yPosition = UnityEngine.Random.Range(0f - height / 2f + 10f, height / 2f - 10f);

                for (int j = 0; j < continents_x_position.Length; j++)
                {
                    if ((Mathf.Abs(continents_x_position[j] - xPosition) < 21) && (Mathf.Abs(continents_y_position[j] - yPosition) < 21))
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
            } while (!flag);


            continents_x_position[i] = xPosition;
            continents_y_position[i] = yPosition;

            GameObject continent_object = Instantiate(continent, new Vector3(xPosition, yPosition, -2), Quaternion.identity);
            continent_object.name = "Continent";
            gameObject.layer = 5;
            continent_object_array.Add(continent_object);
        }



    }





    void country_creator()
    {

        country_count = my_map.get_total_country_count();


        countries_x_position = new float[country_count];
        countries_y_position = new float[country_count];


        int country_counter = 0;
        int current_country_id = 0;

        for (int k = 0; k < continent_count; k++)
        {
            GameObject continent_object = continent_object_array[k] as GameObject;

            float continent_center_x = continent_object.transform.position.x;
            float continent_center_y = continent_object.transform.position.y;

            ArrayList continent_countries = my_continents.get_a_continent_countries(k);

            for (int i = 0; i < continent_countries.Count; i++)
            {
                current_country_id = Int32.Parse(continent_countries[i] as string);
                float yPosition = 0;
                float xPosition = 0;
                bool flag = true;
                do
                {
                    xPosition = UnityEngine.Random.Range(0f - 20 / 2f + 2, 20 / 2f - 2);
                    yPosition = UnityEngine.Random.Range(0f - 20 / 2f + 2, 20 / 2f - 2);

                    for (int j = 0; j < countries_x_position.Length; j++)
                    {
                        if ((Mathf.Abs(countries_x_position[j] - (xPosition + continent_center_x)) < 2.5f)
                            && (Mathf.Abs(countries_y_position[j] - (yPosition + continent_center_y)) < 2.5f))
                        {
                            flag = true;
                            break;
                        }
                        flag = false;
                    }

                } while (flag);




                GameObject country_object = Instantiate(country, new Vector3(xPosition + continent_center_x, yPosition + continent_center_y, -3), Quaternion.identity);
                gameObject.layer = 5;
                country_script = country_object.GetComponent("CountryHandler") as CountryHandler;
                //country_script.create_country(my_map, current_country_id);
                country_object.name = "Country " + current_country_id.ToString();
                countries_x_position[country_counter] = xPosition + continent_center_x;
                countries_y_position[country_counter] = yPosition + continent_center_y;


                //GameObject.Find("Game").GetComponent<GameScript>().add_to_country_coordinates(xPosition + continent_center_x, yPosition + continent_center_y, current_country_id);
                country_object_array.Add(country_object);


                //(GameObject.Find("Country " + country_counter.ToString()).GetComponent("CountryHandler") as CountryHandler).create_country();
                country_counter += 1;
            }
        }




        
    }



    public ArrayList get_countries_list()
    {
        //Debug.Log("Country Length = " + country_object_array.Count);
        return country_object_array;
    }


    



}
