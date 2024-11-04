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
            { "80A", new mineralInfo(0 , "리튬" , "Li" ,"80I") },
            { "80B", new mineralInfo(1 , "나트륨", "Na","80J")  },
            { "80C", new mineralInfo(2 , "마그네슘", "Mg","80K")  },
            { "80D", new mineralInfo(3 , "칼륨", "K","80L")  },
            { "80E", new mineralInfo(4 , "칼슘", "Ca","80M")  },
            { "80F", new mineralInfo(5 , "구리", "Cu","80N")  },
            { "80G", new mineralInfo(6 , "스트론튬", "Sr","80O")  },
            { "80H", new mineralInfo(7 , "바륨", "Ba","80P")  }
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
            Debug.Log("키 존재하지않음");
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
        //기존에 켜져있던 불꽃 끄기
        //새로받은데이터에 맞는 불꽃 키기
    }
    private void ChangeFlame(string data)
    {
        SerialPortManager.Instance.SendData(currentMineral.offCode);
        flames[currentMineral.MeneralId].Stop();
    
        //기존에있던 불꽃제거하고 새로운 불꽃 키기
        //위치
        //어떤오브젝트(불꽃)을 사용할지

    }
}
