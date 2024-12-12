using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIActiveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ui;


    public void SetActiveUI(int mineralIndex)
    {
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].SetActive(false);
        }
        if (mineralIndex != 7)
        {
            ui[mineralIndex].SetActive(true);

        }
    }
}
