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
    private CancellationTokenSource cancellationTokenSource; // CancellationTokenSource �߰�
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
        // ��Ʈ ����

        Debug.Log("��Ʈ����õ�");
        serialPort.ReadTimeout = 50;
        serialPort.Open();
        if (serialPort.IsOpen)
        {
            StartSerialPortReader();
        }
    }


    // ������ �б�
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
                // �����͸� ����

                string input = await Task.Run(() => ReadSerialData() , token);
                string data = GetData(input);

                if (!string.IsNullOrEmpty(data) && data.Length >= 3)
                {
                    Debug.Log("���������� : " + data);
                    controller.ProcessReceivedData(data);
                }

            }
            catch (TimeoutException ex)
            {
                // �����Ͱ� ���� ���� ����
                Debug.LogWarning("������ ���� �ð� �ʰ�: " + ex.Message);
            }
        }
    }
    private string ReadSerialData()
    {
        try
        {
           
            string input = serialPort.ReadExisting().Trim(); // ������ �б�
            return GetData(input);
            //return serialPort.ReadLine(); // ������ �б�
        }
        catch (TimeoutException)
        {
            return null; // �ð� �ʰ� �� null ��ȯ
        }
    }
    private string GetData(string input)
    {
        //input �����Ͱ� 80�����ԵǸ� 80�� �״������ڿ� �ش��ϴ� ���ڿ� ��ŭ ��ȯ.
        if (input.Contains("80"))
        {
            int index = input.IndexOf("80");
            int trimmedDataLanght = 4;
            if (trimmedDataLanght -1 < input.Length)
            {
            // "80" ���� ���ڱ��� �����Ͽ� �ڸ���    
            //Debug.Log(data.Substring(index, 3));
            return input.Substring(index+2  , trimmedDataLanght);
            }
        }
        return "";
    }

    void OnApplicationQuit()
    {
        // ��Ʈ �ݱ�

        // ���� �� ������ ���� �� ��Ʈ �ݱ�

        if (cancellationTokenSource != null)
        {
            Debug.Log("Task ����");
            cancellationTokenSource.Cancel(); // �۾� ���
        }
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

    }

}
