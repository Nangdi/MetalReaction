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
    private Queue<string> dataQueue = new Queue<string>(); // ������ ť
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
        // ��Ʈ ����

        Debug.Log("��Ʈ����õ�");
        serialPort.ReadTimeout = 50;
        serialPort.Open();
        if (serialPort.IsOpen)
        {
            isRunning = true;
            readThread = new Thread(ReadSerialData);
            readThread.Start();
        }
    }


    // ������ �б�
    void Update()
    {
        if (dataQueue.Count > 0)
        {
            string receivedData = dataQueue.Dequeue(); // ť���� �ϳ��� ������ ó��

            if (!string.IsNullOrEmpty(receivedData) && receivedData.Length>=3)
            {
                Debug.Log("���ŵ� ������: " + receivedData);
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
                // �����͸� ����

                string data = serialPort.ReadExisting().Trim();
                if (!string.IsNullOrEmpty(data))
                {
                    // ť�� ���ŵ� ������ �߰�
                    lock (dataQueue) // ��Ƽ������ ȯ�濡�� ť�� �����ϰ� �ٷ�� ���� lock ���
                    {
                        dataQueue.Enqueue(data);
                    }
                }
            }
            catch (TimeoutException ex)
            {
                // �����Ͱ� ���� ���� ����
                Debug.LogWarning("������ ���� �ð� �ʰ�: " + ex.Message);
            }
        }
    }
  


    void OnApplicationQuit()
    {
        // ��Ʈ �ݱ�

        // ���� �� ������ ���� �� ��Ʈ �ݱ�
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
