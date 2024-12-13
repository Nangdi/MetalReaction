using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIFade : MonoBehaviour
{
    [SerializeField]
    private Image ui;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        ui.DOFade(0, 0);
        ui.DOFade(1, 3);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        ui.DOFade(0, 0);
    }
}
