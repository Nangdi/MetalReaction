using System;
using System.IO.Ports;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    public static SerialPortManager Instance { get; private set; }

    [SerializeField]
    private FlameController controller;
    SerialPort serialPort = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);

    private string receiveBuffer = "";
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // 포트 열기
        if (!serialPort.IsOpen)
        {
            Debug.Log("포트연결중");
            serialPort.Open();
            serialPort.ReadTimeout = 1000;
        }
    }

    void Update()
    {
        // 데이터 읽기
        if (serialPort.IsOpen)
        {
            try
            {
                
                string data = serialPort.ReadExisting().Trim(); // 수신된 모든 데이터 읽기
                if (data.Contains("\n"))
                {
                    Debug.Log("줄바꿈포함됨");
                }
                if (!string.IsNullOrEmpty(data))
                {
                    receiveBuffer += data;
                    if(receiveBuffer.Length == 3)
                    {
                        Debug.Log("Received: " + receiveBuffer); // 데이터가 있을 경우 로그 출력
                        controller.ProcessReceivedData(receiveBuffer);
                        receiveBuffer = "";
                    }
                  
                }
                else
                {
                    //Debug.Log("수신된데이터없음");
                }
            }
            catch (TimeoutException ex)
            {
                Debug.LogWarning("데이터 수신 시간 초과: " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("오류 발생: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("연결안됌");
        }
    }
    public void SendData(string message)
    {
        if (serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(message); // 메시지 송신 (줄 바꿈 추가)
                Debug.Log("Sent: " + message);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("송신 오류: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("포트가 열려 있지 않음 - 송신 실패");
        }
    }


    void OnApplicationQuit()
    {
        // 포트 닫기
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}
