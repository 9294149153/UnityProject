using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectiveCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObject;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject gameObject in visualGameObject)
        {
            gameObject.SetActive(true);
        }
        

    }

    private void Hide()
    {
        foreach (GameObject gameObject in visualGameObject)
        {
            gameObject.SetActive(false);
        }
    }
        
}
