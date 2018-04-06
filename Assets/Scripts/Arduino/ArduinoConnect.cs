using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class ArduinoConnect : MonoBehaviour {

    /* The serial port where the Arduino is connected. */
    [Tooltip("The serial port where the Arduino is connected")]
    public string m_port = "COM4";
    /* The baudrate of the serial port. */
    [Tooltip("The baudrate of the serial port")]
    public int m_baudrate = 9600;

    private SerialPort m_stream;

    public void WriteToArduino(string message)
    {
        // Send the request
        m_stream.WriteLine(message);
        m_stream.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 0)
    {
        m_stream.ReadTimeout = timeout;
        try
        {
            return m_stream.ReadLine();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            // A single read attempt
            try
            {
                dataString = m_stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);
            

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    public void Start()
    {
        m_stream = new SerialPort(m_port, m_baudrate);
        m_stream.ReadTimeout = 2;
        m_stream.Open();
    }

    private void OnApplicationQuit()
    {
        m_stream.Close();
    }

}
