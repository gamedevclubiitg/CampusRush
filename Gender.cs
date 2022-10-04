using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gender : MonoBehaviour
{
    [SerializeField]
    public Dropdown gender;
    [SerializeField]
    InputField nameField;
    public string Name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gender.RefreshShownValue();
    }

    public void TakeName()
    {
        Name = nameField.text;
        GameObject.Find("enter name panel").SetActive(false);
    }
}
