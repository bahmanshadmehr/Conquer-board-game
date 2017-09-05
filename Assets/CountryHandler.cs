using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryHandler : MonoBehaviour {

    public Country country;

    //This Class was made to store country object for each instance of instantiated country

    public void create_country(Map map, int id)
    {
        country = new Country(map, id);
        Debug.Log("Country " + id + "WasCreated;");
    }


}
