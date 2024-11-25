using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

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

                string data = await Task.Run(() => ReadSerialData() , token);
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
            return serialPort.ReadExisting().Trim(); // ������ �б�
        }
        catch (TimeoutException)
        {
            return null; // �ð� �ʰ� �� null ��ȯ
        }
    }


    void OnApplicationQuit()
    {
        // ��Ʈ �ݱ�

        // ���� �� ������ ���� �� ��Ʈ �ݱ�

        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // �۾� ���
        }
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

    }

}
