using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentClick : MonoBehaviour
{

    private GameObject continent;
    public Camera cam;

    // Use this for initialization
    void Start()
    {
        continent = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string s = (cam.ScreenToWorldPoint(new Vector3(0, Screen.width, 10)).y + cam.orthographicSize).ToString();

            }
        }
    }
}
