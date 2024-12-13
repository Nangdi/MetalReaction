using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ReactionFlameMovement : MonoBehaviour
{
    [SerializeField]
    private FlameController flameController;
    [Header("�Ҳ� �����ϼ� �ִ� ���ݺ���")]
    [SerializeField]
    private GameObject[] spacingObjects;
    public float spacing =0.5f;

    private Transform targetPos;
    public bool isSpacingSetting = false;
    [SerializeField]
    private GameObject guideText2;
    [Header("�Ҳ� �̵��ӵ�")]
    public float speed = 100;
    public float setSpeed = 5;
    void Start()
    {
        targetPos = flameController.targetPos;
        JsonDataInit();
        ArrangeObject();
    }

    // Update is called once per frame
    void Update()
    {

        if (isSpacingSetting)
        {
            SetPostion();
            SetSpacing();
        }
        else
        {
           

        }
        MoveTargetPos();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacingSetting = !isSpacingSetting;
            guideText2.SetActive(isSpacingSetting);
            if (isSpacingSetting)
            {
                Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("SpacingObject"));
            }
            else
            {
                Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("SpacingObject"));
            }

        }
    }
    private void MoveTargetPos()
    {
        Debug.Log("�̵�");
        if (flameController.targetPos != null)
        {
            float distance = Vector3.Distance(flameController.targetPos.position, transform.position);
            if (distance > 0.0001f)
            {
                transform.position = Vector3.Lerp(transform.position, flameController.targetPos.position, speed * Time.deltaTime);
            }
        }
    }
    private void SetSpacing()
    {

        spacing += Input.GetAxis("Mouse ScrollWheel") * 5 * Time.deltaTime;
        ArrangeObject();
        



    }
    private void SetPostion()
    {
        // Ű �Է¿� ���� x, y ���� �̵��� ���
        float moveX = Input.GetAxis("Horizontal") * setSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * setSpeed * Time.deltaTime;
        // ������Ʈ ��ġ �̵�
        spacingObjects[0].transform.parent.Translate(moveX, -moveY, 0);
        transform.Translate(moveX, -moveY, 0);
    }
    
    private void ArrangeObject()
    {
        //flameObjects�� �߾�ob�� �������� spaceing(����)��ŭ �Ÿ� �����ִ��ڵ�

        int midIndex = spacingObjects.Length / 2;

        for (int i = 0; i < spacingObjects.Length; i++)
        {
            float offset = (i - midIndex) * spacing;
            spacingObjects[i].transform.localPosition = new Vector3(offset, 0, 0);
        }

    }
    private void JsonDataInit()
    {
        spacing = FlameDataManager.Instance.jsondata.spacing;
        transform.localPosition = FlameDataManager.Instance.jsondata.reactionFlamePos;
        spacingObjects[0].transform.parent.localPosition = new Vector3(0,transform.localPosition.y, 0);
    }
    private void OnDestroy()
    {
        FlameDataManager.Instance.jsondata.spacing = spacing;
    }
}
