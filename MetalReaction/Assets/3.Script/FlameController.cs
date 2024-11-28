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
                //_currentMineral�� null �� ��� : �Ҳ��� ������������
                string previousLayer = _currentMineral?.symbol;
                _currentMineral = value;

                // ������ �ٸ� �̳׶��̸� �Ҳ��� �����ϰ� �� �̳׶��� ���� �Ҳ��� �����մϴ�.
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
        //��Ʈ�ѷ���� Ű����� �׽�Ʈ�ϴ� ���� �ڵ�
        string receivedKey = GetPressedKey();
        if (!string.IsNullOrEmpty(receivedKey))
        {
            ProcessReceivedData(receivedKey);
        }
    }
    //Ű���� �׽�Ʈ��
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
        return ""; // �ƹ� Ű�� ������ �ʾ���7�� ��ȯ�� �⺻ ��
    }
    //�ٷ��� Flames �迭�� MineralInfo�� ID�� �ǳ����� ������
    private void InitializeCommandActions()
    {
        mineralDic = new Dictionary<string, MineralInfo>
       {
            { "80A", new MineralInfo(0 , "��Ƭ" , "Li" ) },
            { "80B", new MineralInfo(1 , "��Ʈ��", "Na")  },
            { "80C", new MineralInfo(2 , "Į��", "K" ) },
            { "80D", new MineralInfo(3 , "Į��", "Ca")  },
            { "80E", new MineralInfo(4 , "����", "Cu")  },
            { "80F", new MineralInfo(5 , "��Ʈ��Ƭ", "Sr")  },
            { "80G", new MineralInfo(6 , "�ٷ�", "Ba")  },
            { "80Z", new MineralInfo(7 , "�⺻", "")  }
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
            Debug.Log("Ű ������������");
            return;
        }
      
       //���籤�� ������Ʈ
      
    }
    
    private void PlayFlame(string layer)
    {
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} ���̾ �߰��Ǿ����ϴ�.");

    }
    private void StopFlame(string layer)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} ���̾ ���ŵǾ����ϴ�.");
      
    }
}
