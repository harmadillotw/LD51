using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    TextMeshProUGUI buttonText;
    string menuText = "";
    float fontsize;

    private void Awake()
    {
        buttonText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        menuText = buttonText.text;
        fontsize = buttonText.fontSize;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.fontSize = buttonText.fontSize + 5;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.fontSize = fontsize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonText.color = new Color32(204, 36, 111, 255);
    }
}
