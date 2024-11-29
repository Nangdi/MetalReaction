using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MineralInfo
{
    public int MineralId { get; private set; }
    public string mineralName { get; private set; }
    public string symbol { get; private set; }

    public string offCode { get; private set; }





    public MineralInfo(int num, string name, string _symbor)
    {
        MineralId = num;
        mineralName = name;
        symbol = _symbor;
    }
}


public class FlameController : MonoBehaviour
{
  

    private MineralInfo _currentMineral;
    public MineralInfo currentMineral
    {
        get { return _currentMineral; }
        //private set
        //{
        //    if (_currentMineral != value)
        //    {
        //        //_currentMineral이 null 인 경우 : 불꽃이 안켜져있을때
        //        string previousLayer = _currentMineral?.symbol;
        //        _currentMineral = value;

        //        // 이전과 다른 미네랄이면 불꽃을 종료하고 새 미네랄에 대한 불꽃을 시작합니다.
        //        if (previousLayer != _currentMineral?.symbol)
        //        {
        //            StopFlame(previousLayer);
        //        }
        //        PlayFlame(_currentMineral.symbol);
        //        uiManager.SetActiveUI(_currentMineral.MineralId);
        //    }
        //}
    }
    [SerializeField]
    private UIManager uiManager;
    private Dictionary<char, MineralInfo> mineralDic;
    [SerializeField]
    private Transform flame;
    [SerializeField]
    private Transform[] flamePos;
    public Transform targetPos;
    [SerializeField]
    private VisualEffect[] flames = new VisualEffect[8];
    private int posData;
    private void Start()
    {
        StartCoroutine(WaitCameraMaskSetting());
        InitializeCommandActions();
        //AllStopEffect();
    }
    private void Update()
    {
        //컨트롤러대신 키보드로 테스트하는 용의 코드
        string receivedKey = GetPressedKey();
        if (!string.IsNullOrEmpty(receivedKey))
        {
            ProcessReceivedData(receivedKey);
        }
    }
    //키보드 테스트용
    string GetPressedKey()
    {
        if (Input.GetKey(KeyCode.Alpha1)) return "3A";
        if (Input.GetKey(KeyCode.Alpha2)) return "3C";
        if (Input.GetKey(KeyCode.Alpha3)) return "3D";
        if (Input.GetKey(KeyCode.Alpha4)) return "3E";
        if (Input.GetKey(KeyCode.Alpha5)) return "3F";
        if (Input.GetKey(KeyCode.Alpha6)) return "3G";
        if (Input.GetKey(KeyCode.Alpha7)) return "3H";
        if (Input.GetKey(KeyCode.Alpha8)) return "3B";
        return ""; // 아무 키도 눌리지 않았을7때 반환할 기본 값
    }
    //바륨의 Flames 배열과 MineralInfo의 ID가 맨끝으로 가야함
    private void InitializeCommandActions()
    {
        mineralDic = new Dictionary<char, MineralInfo>
       {
            { 'A', new MineralInfo(0 , "리튬" , "Li" ) },
            { 'B', new MineralInfo(1 , "나트륨", "Na")  },
            { 'C', new MineralInfo(2 , "칼륨", "K" ) },
            { 'D', new MineralInfo(3 , "칼슘", "Ca")  },
            { 'E', new MineralInfo(4 , "구리", "Cu")  },
            { 'F', new MineralInfo(5 , "스트론튬", "Sr")  },
            { 'G', new MineralInfo(6 , "바륨", "Ba")  },
            { 'Z', new MineralInfo(7 , "기본", "")  }
        };

    }

    private void AllStopEffect()
    {
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].Stop();
        }
    }
    public void ProcessReceivedData(string data)
    {
        if (mineralDic.ContainsKey(data[3]))
        {
            int index = int.Parse(data[2].ToString())-1 ;
            ChangeFlameData(mineralDic[data[3]], index);
            //currentMineral = mineralDic[data[3]];
            if (targetPos != flamePos[index])
            {
                targetPos = flamePos[index];

            }
            posData = index;


        }
        else
        {
            Debug.Log("키 존재하지않음");
            return;
        }
      
       //현재광물 업데이트
      
    }
    
    private void PlayFlame(string layer, int posIndex)
    {
        if (targetPos != flamePos[posIndex])
        {
            targetPos = flamePos[posIndex];

        }
        MoveImmediately(posIndex);
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} 레이어가 추가되었습니다.");
        //MoveImmediately(posData);
    }
    private void StopFlame(string layer)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} 레이어가 제거되었습니다.");
      
    }
    private void MoveImmediately(int index)
    {
        flame.position = flamePos[index].position;
    }
    private void ChangeFlameData(MineralInfo info, int posData)
    {
        if (_currentMineral != info)
        {
            //_currentMineral이 null 인 경우 : 불꽃이 안켜져있을때
            string previousLayer = _currentMineral?.symbol;
            _currentMineral = info;

            // 이전과 다른 미네랄이면 불꽃을 종료하고 새 미네랄에 대한 불꽃을 시작합니다.
            if (previousLayer != _currentMineral?.symbol)
            {
                StopFlame(previousLayer);
            }
            PlayFlame(_currentMineral.symbol, posData);
            uiManager.SetActiveUI(_currentMineral.MineralId);
        }
    }
    private IEnumerator WaitCameraMaskSetting()
    {

        Camera.main.cullingMask = -1;
        yield return new WaitForSeconds(1.5f);
        Camera.main.cullingMask = 0;
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Default"));
    }
}
