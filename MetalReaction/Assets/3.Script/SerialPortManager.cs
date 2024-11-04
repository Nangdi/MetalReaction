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
        // ��Ʈ ����
        if (!serialPort.IsOpen)
        {
            Debug.Log("��Ʈ������");
            serialPort.Open();
            serialPort.ReadTimeout = 1000;
        }
    }

    void Update()
    {
        // ������ �б�
        if (serialPort.IsOpen)
        {
            try
            {
                
                string data = serialPort.ReadExisting().Trim(); // ���ŵ� ��� ������ �б�
                if (data.Contains("\n"))
                {
                    Debug.Log("�ٹٲ����Ե�");
                }
                if (!string.IsNullOrEmpty(data))
                {
                    receiveBuffer += data;
                    if(receiveBuffer.Length == 3)
                    {
                        Debug.Log("Received: " + receiveBuffer); // �����Ͱ� ���� ��� �α� ���
                        controller.ProcessReceivedData(receiveBuffer);
                        receiveBuffer = "";
                    }
                  
                }
                else
                {
                    //Debug.Log("���ŵȵ����;���");
                }
            }
            catch (TimeoutException ex)
            {
                Debug.LogWarning("������ ���� �ð� �ʰ�: " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("���� �߻�: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("����ȉ�");
        }
    }
    public void SendData(string message)
    {
        if (serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(message); // �޽��� �۽� (�� �ٲ� �߰�)
                Debug.Log("Sent: " + message);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("�۽� ����: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("��Ʈ�� ���� ���� ���� - �۽� ����");
        }
    }


    void OnApplicationQuit()
    {
        // ��Ʈ �ݱ�
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}
