using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ReactionFlameMovement : MonoBehaviour
{
    [SerializeField]
    private FlameController flameController;
    [Header("�Ҳ� �����ϼ� �ִ� ���ݺ���")]
    [SerializeField]
    private GameObject[] flameObjects;
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
        spacing= FlameDataManager.Instance.data.spacing;
        transform.localPosition = FlameDataManager.Instance.data.reactionFlamePos;
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
            MoveTargetPos();

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacingSetting = !isSpacingSetting;
            guideText2.SetActive(isSpacingSetting);
            if (isSpacingSetting)
            {
                Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("quad"));
            }
            else
            {
                Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("quad"));
            }

        }
    }
    private void MoveTargetPos()
    {
        if (flameController.targetPos != null)
        {
            float distance = Vector3.Distance(flameController.targetPos.position, transform.position);
            if (distance > 0.0001f)
            {
                Debug.Log("������Ʈ�̵���");
                //transform.Translate(targetPos.position * speed * Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, flameController.targetPos.position, speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, flameController.targetPos.position, speed * Time.deltaTime);
            }
        }
    }
    private void SetSpacing()
    {

        spacing += Input.GetAxis("Mouse ScrollWheel") * 5 * Time.deltaTime;
        //spacing = Mathf.Clamp01(spacing);
        ArrangeObject();
        



    }
    private void SetPostion()
    {
        // Ű �Է¿� ���� x, y ���� �̵��� ���
        float moveX = Input.GetAxis("Horizontal") * setSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * setSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Mouse ScrollWheel") * setSpeed * Time.deltaTime;
        // ������Ʈ ��ġ �̵�
        transform.Translate(moveX, -moveY, moveZ);
    }
    private void ArrangeObject()
    {
        int midIndex = flameObjects.Length / 2;

        for (int i = 0; i < flameObjects.Length; i++)
        {
            float offset = (i - midIndex) * spacing;
            flameObjects[i].transform.localPosition = new Vector3(offset, 0, 0);
        }

    }
    private void OnDestroy()
    {
        FlameDataManager.Instance.data.spacing = spacing;
    }
}
