using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Basketball_leaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private List<HighscoreEntry> highscoreEntryList;

    public GameObject scoreText;
    // public GameObject inputField;
    string userName;
    string endScore;

    private int limitToShow = 100;
    
    public GameObject YourScore, LeaderBoard;

    public void Wake() {
        // entryContainer = transform.Find("highscoreEntryContainer");
        // entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        
        // string jsonString = PlayerPrefs.GetString("ScoreTable_basketball");
        // Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        // Debug.Log(highscores);
        // if (highscores == null) {
        //     // There's no stored table, initialize
        //     Debug.Log("Initializing table with default values...");
        //     AddHighscoreEntry("40", "CMK");
        //     AddHighscoreEntry("35", "JOE");
        //     AddHighscoreEntry("30", "DAV");
        //     AddHighscoreEntry("25", "CAT");
        //     AddHighscoreEntry("20", "MAX");
        //     AddHighscoreEntry("15", "AAA");
        //     // Reload
        //     jsonString = PlayerPrefs.GetString("ScoreTable_basketball");
        //     highscores = JsonUtility.FromJson<Highscores>(jsonString);
        // }

        // // Sort entry list by Score
        // for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
        //     for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
        //         if (float.Parse(highscores.highscoreEntryList[j].score) > float.Parse(highscores.highscoreEntryList[i].score)) {
        //             // Swap
        //             HighscoreEntry tmp = highscores.highscoreEntryList[i];
        //             highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
        //             highscores.highscoreEntryList[j] = tmp;
        //         }
        //     }
        // }

        limitToShow = DBManager.numberOfLeaderboardDataToFetch;

        highscoreEntryTransformList = new List<Transform>();
        highscoreEntryList = new List<HighscoreEntry>();

        WWWForm leaderBoardForm = DBops.GetLeaderboardForm("basket", limitToShow.ToString(), "0");
        Debug.Log("Getting Basketball Leaderboard");
        StartCoroutine(LeaderboardCheck(Link.getLeaderboard, leaderBoardForm));

        // int k = 0;
        // foreach (HighscoreEntry highscoreEntry in highscoreEntryList) {
        //     CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //     k++;
        //     if (k >= 10)
        //         break;
        // }
    }

    IEnumerator LeaderboardCheck(string target, WWWForm leaderBoardForm)
    {
        Debug.Log("coroutine started");
        UnityWebRequest request = UnityWebRequest.Post(target, leaderBoardForm);
        request.SetRequestHeader("User-Agent", "DefaultBrowser"); 
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("returned request");
            Debug.Log(request.downloadHandler.text);
        }

        string[] leaderboardCheckResult = DBops.GetResultFromRequest(request);
        Debug.Log(leaderboardCheckResult[0]);

        if (leaderboardCheckResult[0] == "0")
        {
            for (int i = 1; i < (leaderboardCheckResult.Length - 1); i++)
            {
                string[] entry = leaderboardCheckResult[i].Split(',');
                HighscoreEntry highscoreEntry = new HighscoreEntry{
                    name = entry[0],
                    score = entry[1]
                };

                highscoreEntryList.Add(highscoreEntry);
            }
        }

        
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0,25 -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        // entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        // if (rank == 1) {
        //     entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
        //     entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
        //     entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        // }

        // Set tropy
        // switch (rank) {
        // default:
        //     entryTransform.Find("trophy").gameObject.SetActive(false);
        //     break;
        // case 1:
        //     entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
        //     break;
        // case 2:
        //     entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
        //     break;
        // case 3:
        //     entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
        //     break;

        // }

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(string score, string name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("ScoreTable_basketball");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("ScoreTable_basketball", json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    public void AddNewEntry(){
        AddHighscoreEntry(scoreText.GetComponent<Text>().text, userName);
    }
    public void readStringInput(string s){
        
        // userName=inputField.GetComponent<Text>().text;

        Debug.Log(scoreText.GetComponent<Text>().text);
        // updateLeaderBoard();
        Debug.Log(userName);
    }

    public void View_LeaderBoard(){
        YourScore.SetActive(false);
        LeaderBoard.SetActive(true);
    }

    public void back_button(){
        LeaderBoard.SetActive(false);
        YourScore.SetActive(true);
    }
    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    private class HighscoreEntry {
        public string score;
        public string name;
    }
}
