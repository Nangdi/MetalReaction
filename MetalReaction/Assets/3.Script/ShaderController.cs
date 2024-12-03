using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Material shaderMaterial;  // Shader가 적용된 Material
    private float initialPlayTime = 0f;  // 초기화할 Playtime 값
    float playTime;

    void Start()
    {
        // 초기화 시 Playtime을 0으로 설정
        ResetPlaytime();
    }

    void Update()
    {
        // 매 프레임마다 _PlayTime 값을 업데이트
        if (playTime < 1.5f)
        {
            playTime += Time.deltaTime; // 실제 시간값을 넣을 수 있습니다.
            shaderMaterial.SetFloat("_PlayTime", playTime);

        }
        Debug.Log(playTime);
    }

    // Shader의 Playtime 초기화 함수
    public void ResetPlaytime()
    {
        // _PlayTime을 0으로 설정하여 초기화
        shaderMaterial.SetFloat("_PlayTime", initialPlayTime);
        playTime = initialPlayTime;
    }

    private void OnEnable()
    {
        ResetPlaytime();
    }
}

