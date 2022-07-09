using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableUI : MonoBehaviour
{
    public Transform popUP;
    public GameObject popUpAsGameObject;
    public Image buttonIcon;
    public TextMeshProUGUI interactableText;
    public Image itemIcon;

    private void Start()
    {
        popUP = transform.GetChild(0);
        popUpAsGameObject = transform.GetChild(0).gameObject;
    
        interactableText = popUP.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemIcon = popUP.transform.GetChild(1).GetComponent<Image>();
        buttonIcon = popUP.transform.GetChild(2).GetComponent<Image>();
        
        popUP.gameObject.SetActive(false);
    }
}