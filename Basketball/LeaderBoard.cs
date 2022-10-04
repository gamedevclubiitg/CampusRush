using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake(){
        entryContainer=transform.Find("EntryContainer");
        entryTemplate=entryContainer.Find("EntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        float templateHeight = 20f;
        for(int i=0; i<10; i++){
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition=new Vector2(0, -templateHeight*i);
            entryTransform.gameObject.SetActive(true);

            int rank = i+1;
            string rankString;
            switch (rank){
                default:
                rankString=rank+"TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            // entryTransform.Find("Position").GetComponent<Text>().text = "";
            // entryTransform.Find("PlayerScore").GetComponent<Text>().text = "";
            // entryTransform.Find("PlayerName").GetComponent<Text>().text = "";
        }

        

    }
    
}
