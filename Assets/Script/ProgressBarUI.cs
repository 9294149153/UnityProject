using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
  [SerializeField]  private Image barImage;



    private void Start()
    {
        cuttingCounter.OnPorgressChange += CuttingCounter_OnPorgressChange;
        barImage.fillAmount = 0f;

        Hide();
    }

    private void CuttingCounter_OnPorgressChange(object sender, CuttingCounter.OnOnPorgressChangeEventArgs e)
    {

        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
