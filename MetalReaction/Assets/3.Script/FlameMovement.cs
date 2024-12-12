using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FlameMovement : MonoBehaviour
{
    public float speed = 5.0f;  // 이동 속도
 
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
        // 키 입력에 따라 x, y 방향 이동량 계산
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime;
        // 오브젝트 위치 이동
        transform.Translate(moveX, -moveY, moveZ);
    }
    private void OnDisable()
    {
        FlameDataManager.Instance.jsondata.flamePos = transform.position;
    }

}
