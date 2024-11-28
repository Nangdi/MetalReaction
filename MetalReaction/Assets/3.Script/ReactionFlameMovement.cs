using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ReactionFlameMovement : MonoBehaviour
{
    [SerializeField]
    private FlameController flameController;
    private Transform targetPos;
    public float speed = 100;
    void Start()
    {
        targetPos = flameController.targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (flameController.targetPos != null)
        {
            float distance = Vector3.Distance(flameController.targetPos.position, transform.position);
            if (distance > 0.0001f)
            {
                Debug.Log("오브젝트이동중");
                //transform.Translate(targetPos.position * speed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, flameController.targetPos.position, speed * Time.deltaTime);
            }
        }

    }
}
