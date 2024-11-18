using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    public static SerialPortManager Instance { get; private set; }

    [SerializeField]
    private FlameController controller;
    SerialPort serialPort = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);
    private Queue<string> dataQueue = new Queue<string>(); // 데이터 큐
    private bool isRunning = false;
    private Thread readThread;
    private void Awake()
    {
        if (Instance == null)
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

        Debug.Log("포트연결시도");
        serialPort.ReadTimeout = 50;
        serialPort.Open();
        if (serialPort.IsOpen)
        {
            isRunning = true;
            readThread = new Thread(ReadSerialData);
            readThread.Start();
        }
    }


    // 데이터 읽기
    void Update()
    {
        if (dataQueue.Count > 0)
        {
            string receivedData = dataQueue.Dequeue(); // 큐에서 하나씩 꺼내서 처리

            if (!string.IsNullOrEmpty(receivedData) && receivedData.Length>=3)
            {
                Debug.Log("수신된 데이터: " + receivedData);
                controller.ProcessReceivedData(receivedData);
            }
        }
    }
    void ReadSerialData()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // 데이터를 수신

                string data = serialPort.ReadExisting().Trim();
                if (!string.IsNullOrEmpty(data))
                {
                    // 큐에 수신된 데이터 추가
                    lock (dataQueue) // 멀티스레드 환경에서 큐를 안전하게 다루기 위해 lock 사용
                    {
                        dataQueue.Enqueue(data);
                    }
                }
            }
            catch (TimeoutException ex)
            {
                // 데이터가 없을 때는 무시
                Debug.LogWarning("데이터 수신 시간 초과: " + ex.Message);
            }
        }
    }
  


    void OnApplicationQuit()
    {
        // 포트 닫기

        // 종료 시 쓰레드 정리 및 포트 닫기
        isRunning = false;
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

    }

}
