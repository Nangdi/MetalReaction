using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//using static UnityEditor.Experimental.GraphView.GraphView;

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
        private set
        {
            if (_currentMineral != value)
            {
                //_currentMineral이 null 인 경우 : 불꽃이 안켜져있을때
                string previousLayer = _currentMineral?.symbol;
                _currentMineral = value;

                // 이전과 다른 미네랄이면 불꽃을 종료하고 새 미네랄에 대한 불꽃을 시작합니다.
                if (previousLayer != _currentMineral?.symbol)
                {
                    StopFlame(previousLayer);
                }
                PlayFlame(_currentMineral.symbol);
                uiManager.SetActiveUI(_currentMineral.MineralId);
            }
        }
    }
    [SerializeField]
    private UIManager uiManager;
    private Dictionary<string, MineralInfo> mineralDic;
    [SerializeField]
    private VisualEffect[] flames = new VisualEffect[8];
    
    private void Start()
    {
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
        if (Input.GetKey(KeyCode.Alpha1)) return "80A";
        if (Input.GetKey(KeyCode.Alpha2)) return "80C";
        if (Input.GetKey(KeyCode.Alpha3)) return "80D";
        if (Input.GetKey(KeyCode.Alpha4)) return "80E";
        if (Input.GetKey(KeyCode.Alpha5)) return "80F";
        if (Input.GetKey(KeyCode.Alpha6)) return "80G";
        if (Input.GetKey(KeyCode.Alpha7)) return "80H";
        if (Input.GetKey(KeyCode.Alpha8)) return "80B";
        return ""; // 아무 키도 눌리지 않았을7때 반환할 기본 값
    }
    //바륨의 Flames 배열과 MineralInfo의 ID가 맨끝으로 가야함
    private void InitializeCommandActions()
    {
        mineralDic = new Dictionary<string, MineralInfo>
       {
            { "80A", new MineralInfo(0 , "리튬" , "Li" ) },
            { "80B", new MineralInfo(1 , "나트륨", "Na")  },
            { "80C", new MineralInfo(2 , "칼륨", "K" ) },
            { "80D", new MineralInfo(3 , "칼슘", "Ca")  },
            { "80E", new MineralInfo(4 , "구리", "Cu")  },
            { "80F", new MineralInfo(5 , "스트론튬", "Sr")  },
            { "80G", new MineralInfo(6 , "바륨", "Ba")  },
            { "80Z", new MineralInfo(7 , "기본", "")  }
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
        //data = data.Substring(0, 3);
        if (mineralDic.ContainsKey(data))
        {
            currentMineral = mineralDic[data];
            
        }
        else
        {
            Debug.Log("키 존재하지않음");
            return;
        }
      
       //현재광물 업데이트
      
    }
    
    private void PlayFlame(string layer)
    {
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} 레이어가 추가되었습니다.");

    }
    private void StopFlame(string layer)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} 레이어가 제거되었습니다.");
      
    }
}
