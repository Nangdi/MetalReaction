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
        //        //_currentMineral�� null �� ��� : �Ҳ��� ������������
        //        string previousLayer = _currentMineral?.symbol;
        //        _currentMineral = value;

        //        // ������ �ٸ� �̳׶��̸� �Ҳ��� �����ϰ� �� �̳׶��� ���� �Ҳ��� �����մϴ�.
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
        if (Input.GetKey(KeyCode.Alpha1)) return "3A";
        if (Input.GetKey(KeyCode.Alpha2)) return "3C";
        if (Input.GetKey(KeyCode.Alpha3)) return "3D";
        if (Input.GetKey(KeyCode.Alpha4)) return "3E";
        if (Input.GetKey(KeyCode.Alpha5)) return "3F";
        if (Input.GetKey(KeyCode.Alpha6)) return "3G";
        if (Input.GetKey(KeyCode.Alpha7)) return "3H";
        if (Input.GetKey(KeyCode.Alpha8)) return "3B";
        return ""; // �ƹ� Ű�� ������ �ʾ���7�� ��ȯ�� �⺻ ��
    }
    //�ٷ��� Flames �迭�� MineralInfo�� ID�� �ǳ����� ������
    private void InitializeCommandActions()
    {
        mineralDic = new Dictionary<char, MineralInfo>
       {
            { 'A', new MineralInfo(0 , "��Ƭ" , "Li" ) },
            { 'B', new MineralInfo(1 , "��Ʈ��", "Na")  },
            { 'C', new MineralInfo(2 , "Į��", "K" ) },
            { 'D', new MineralInfo(3 , "Į��", "Ca")  },
            { 'E', new MineralInfo(4 , "����", "Cu")  },
            { 'F', new MineralInfo(5 , "��Ʈ��Ƭ", "Sr")  },
            { 'G', new MineralInfo(6 , "�ٷ�", "Ba")  },
            { 'Z', new MineralInfo(7 , "�⺻", "")  }
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
            Debug.Log("Ű ������������");
            return;
        }
      
       //���籤�� ������Ʈ
      
    }
    
    private void PlayFlame(string layer, int posIndex)
    {
        if (targetPos != flamePos[posIndex])
        {
            targetPos = flamePos[posIndex];

        }
        MoveImmediately(posIndex);
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} ���̾ �߰��Ǿ����ϴ�.");
        //MoveImmediately(posData);
    }
    private void StopFlame(string layer)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        //Debug.Log($"{layer} ���̾ ���ŵǾ����ϴ�.");
      
    }
    private void MoveImmediately(int index)
    {
        flame.position = flamePos[index].position;
    }
    private void ChangeFlameData(MineralInfo info, int posData)
    {
        if (_currentMineral != info)
        {
            //_currentMineral�� null �� ��� : �Ҳ��� ������������
            string previousLayer = _currentMineral?.symbol;
            _currentMineral = info;

            // ������ �ٸ� �̳׶��̸� �Ҳ��� �����ϰ� �� �̳׶��� ���� �Ҳ��� �����մϴ�.
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
