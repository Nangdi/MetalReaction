using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FlameMovement : MonoBehaviour
{
    public float speed = 5.0f;  // �̵� �ӵ�
 
    private bool isPositionSetting = false;
    [SerializeField]
    private GameObject guideText;
   
 
    private void Start()
    {
        transform.position =FlameDataManager.Instance.jsondata.flamePos;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPositionSetting = !isPositionSetting;
            guideText.SetActive(isPositionSetting);
        }
        
        if (isPositionSetting)
        {
            SetPostion();
        }
    }
  
    private void SetPostion()
    {
        // Ű �Է¿� ���� x, y ���� �̵��� ���
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime;
        // ������Ʈ ��ġ �̵�
        transform.Translate(moveX, -moveY, moveZ);
    }
    private void OnDisable()
    {
        FlameDataManager.Instance.jsondata.flamePos = transform.position;
    }

}
