using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class mineralInfo
{
    public int MeneralId { get; set; }
    public string mineralName { get; set; }
    public string symbol { get; set; }

    public string offCode { get; set; }

    public mineralInfo(int num, string name ,string _symbor , string _offCode)
    {
        MeneralId = num;
        mineralName = name;
        symbol = _symbor;
        offCode = _offCode;
    }




} 


public class FlameController : MonoBehaviour
{
    private Dictionary<string, mineralInfo> mineralDic;

    private mineralInfo currentMineral;
    //{
    //    set
    //    {

    //    }
    //}
    [SerializeField]
    private VisualEffect[] flames = new VisualEffect[8];
    private void Start()
    {
        InitializeCommandActions();
        //AllStopEffect();
    }

    private void InitializeCommandActions()
    {
        mineralDic = new Dictionary<string, mineralInfo>
        {
            { "80A", new mineralInfo(0 , "��Ƭ" , "Li" ,"80I") },
            { "80B", new mineralInfo(1 , "��Ʈ��", "Na","80J")  },
            { "80C", new mineralInfo(2 , "���׳׽�", "Mg","80K")  },
            { "80D", new mineralInfo(3 , "Į��", "K","80L")  },
            { "80E", new mineralInfo(4 , "Į��", "Ca","80M")  },
            { "80F", new mineralInfo(5 , "����", "Cu","80N")  },
            { "80G", new mineralInfo(6 , "��Ʈ��Ƭ", "Sr","80O")  },
            { "80H", new mineralInfo(7 , "�ٷ�", "Ba","80P")  }
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
        if (!mineralDic.ContainsKey(data))
        {
            Debug.Log("Ű ������������");
            return;
        }
        if(currentMineral != null && currentMineral != mineralDic[data])
        {
            
            ChangeFlame(data);
        }
        currentMineral = mineralDic[data];
        flames[currentMineral.MeneralId].Play();
        Debug.Log(currentMineral.offCode);
        Debug.Log(currentMineral.mineralName);
        //������ �����ִ� �Ҳ� ����
        //���ι��������Ϳ� �´� �Ҳ� Ű��
    }
    private void ChangeFlame(string data)
    {
        SerialPortManager.Instance.SendData(currentMineral.offCode);
        flames[currentMineral.MeneralId].Stop();
    
        //�������ִ� �Ҳ������ϰ� ���ο� �Ҳ� Ű��
        //��ġ
        //�������Ʈ(�Ҳ�)�� �������

    }
}
