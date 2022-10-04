using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;



public class Info : MonoBehaviour
{
    [SerializeField]
    GameObject activePanel;
    Text LocationName;
    Text LocationInfo;
    // Start is called before the first frame update
    void Start()
    {
        LocationInfo=GameObject.Find("LocationInfo").GetComponent<Text>();
        LocationName=GameObject.Find("LocationName").GetComponent<Text>();   
        activePanel=GameObject.Find("InformationUI");
        activePanel.SetActive(false);
        Debug.Log("Chal gaya");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            activePanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if (IsLocalPlayer)
        {
            switch (other.name)
            {
                case "testLocation":
                    activePanel.SetActive(true);
                    LocationName.text="";
                    LocationInfo.text="";
                    break;

                case "NewSacTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="New Sac";
                    LocationInfo.text="New sac houses all the clubs in the institute. Once in campus you will find yourself spending most of your time here";
                    break;

                case "SportsComplexTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Sports Complex";
                    LocationInfo.text="Sport's Complex comprises of the fields, courts and rooms of 10+ different sports. If you wanna play Basketball and \nFootball to Squash and Table- Tennis, or spend some time at the gym, this is the place to be. ";
                    break;
                
                case "AlcherStageTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Alcher Stage";
                    LocationInfo.text="Heard about Alcheringa? Guess where it's held";
                    break;

                case "LohitTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Lohit Hostel";
                    LocationInfo.text="A co-ed hostel located quite close to Siang, overlooking the cricket ground. On the west side, you can watch the sun \nset every evening behind the academic office casting its dying orange reflection on the IITG lake, truly mesmerizing. ";
                    break;

                case "SiangTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Siang Hostel";
                    LocationInfo.text="A relatively small hostel at the most prime location. Being adjacent to cricket ground, watching matches from balcony gives feeling of sitting in pavilion. IITG lake, academic office, central library are visible from this hostel.";
                    break;

                case "DisangTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Disang Hostel";
                    LocationInfo.text="It is the newest hostel in the campus and was inaugurated recently. According to the few people who have eaten in the mess there the food is among the best in the campus.";
                    break;

                case "CricketGroundTrigger":
                    activePanel.SetActive(true);
                    LocationName.text="Cricket Ground";
                    LocationInfo.text="As the name suggests it is a ground used to play cricket :) Also the seating area is sometimes used as a open air theatre to watch movies";
                    break;
                

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }
}
