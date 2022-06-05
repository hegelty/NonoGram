using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartGameButon : MonoBehaviour, IPointerEnterHandler
{
    public GameObject selectGamePannel;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectGamePannel.SetActive(true);
    }
}
