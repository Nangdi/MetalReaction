using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Material shaderMaterial;  // Shader�� ����� Material
    private float initialPlayTime = 0f;  // �ʱ�ȭ�� Playtime ��
    float playTime;

    void Start()
    {
        // �ʱ�ȭ �� Playtime�� 0���� ����
        ResetPlaytime();
    }

    void Update()
    {
        // �� �����Ӹ��� _PlayTime ���� ������Ʈ
        if (playTime < 1.5f)
        {
            playTime += Time.deltaTime; // ���� �ð����� ���� �� �ֽ��ϴ�.
            shaderMaterial.SetFloat("_PlayTime", playTime);

        }
        Debug.Log(playTime);
    }

    // Shader�� Playtime �ʱ�ȭ �Լ�
    public void ResetPlaytime()
    {
        // _PlayTime�� 0���� �����Ͽ� �ʱ�ȭ
        shaderMaterial.SetFloat("_PlayTime", initialPlayTime);
        playTime = initialPlayTime;
    }

    private void OnEnable()
    {
        ResetPlaytime();
    }
}

