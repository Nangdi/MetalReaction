using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class SerialPortManager : MonoBehaviour
{
    public static SerialPortManager Instance { get; private set; }

    [SerializeField]
    private FlameController controller;
    SerialPort serialPort = new SerialPort("COM7", 19200, Parity.None, 8, StopBits.One);
    private CancellationTokenSource cancellationTokenSource; // CancellationTokenSource 추가
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
            StartSerialPortReader();
        }
    }


    // 데이터 읽기
    void Update()
    {
    
    }
    async void StartSerialPortReader()
    {
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        while (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // 데이터를 수신

                string input = await Task.Run(() => ReadSerialData() , token);
                string data = GetData(input);

                if (!string.IsNullOrEmpty(data) && data.Length >= 3)
                {
                    Debug.Log("받은데이터 : " + data);
                    controller.ProcessReceivedData(data);
                }

            }
            catch (TimeoutException ex)
            {
                // 데이터가 없을 때는 무시
                Debug.LogWarning("데이터 수신 시간 초과: " + ex.Message);
            }
        }
    }
    private string ReadSerialData()
    {
        try
        {
           
            string input = serialPort.ReadExisting().Trim(); // 데이터 읽기
            return GetData(input);
            //return serialPort.ReadLine(); // 데이터 읽기
        }
        catch (TimeoutException)
        {
            return null; // 시간 초과 시 null 반환
        }
    }
    private string GetData(string input)
    {
        //input 데이터가 80이포함되면 80과 그다음문자에 해당하는 문자열 만큼 반환.
        if (input.Contains("80"))
        {
            int index = input.IndexOf("80");
            int trimmedDataLanght = 4;
            if (trimmedDataLanght -1 < input.Length)
            {
            // "80" 다음 문자까지 포함하여 자르기    
            //Debug.Log(data.Substring(index, 3));
            return input.Substring(index+2  , trimmedDataLanght);
            }
        }
        return "";
    }

    void OnApplicationQuit()
    {
        // 포트 닫기

        // 종료 시 쓰레드 정리 및 포트 닫기

        if (cancellationTokenSource != null)
        {
            Debug.Log("Task 종료");
            cancellationTokenSource.Cancel(); // 작업 취소
        }
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

    }

}
