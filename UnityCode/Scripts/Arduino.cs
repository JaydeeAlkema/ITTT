using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class Arduino : MonoBehaviour
{
	// Stream with which we connect to the Arduino.
	private SerialPort stream;

	[SerializeField]
	[Tooltip("If enabled all the debug.log will be run. Turn of for improved performance")]
	private bool debugMode;

	[SerializeField]
	[Tooltip("If enabled the connection with the Arduino is succesfull.")]
	private bool connectionOpened;

	[SerializeField]
	[Tooltip("The port wit which the Arduino is connected.")]
	private string port = "COM8";

	[SerializeField]
	[Tooltip("The Baud Rate of the port. Make sure this is the same as in the Arduino IDE.")]
	private int baudrate = 9600;

	[SerializeField]
	[Tooltip("This is the 'delay' of the readout of the port. This should also be the same as in the Arduino IDE.")]
	[Range(0, 50)]
	private int readTimeout = 50;

	[Space]
	//	---------------------- CUSTOM VARIABLES ---------------------- \\
	[SerializeField] private float gyroAccel;	// Acceleration of the MPU6050
	[SerializeField] private Vector3 rotation;  // Rotation of the MPU6050
	[SerializeField] private Vector3 offset;    // Offset for the rotation
	//	---------------------- CUSTOM VARIABLES ---------------------- \\

	public Vector3 Rotation { get => rotation; set => rotation = value; }
	public float GyroAccel { get => gyroAccel; set => gyroAccel = value; }

	// Start the Arduino script
	public void Start()
	{
		if(debugMode) Debug.Log("Code Started");
		Thread arduinoThread = new Thread(new ThreadStart(OpenConnection));
		arduinoThread.IsBackground = true;
		arduinoThread.Start();
	}

	// Always read data from the arduino (Duhh)
	private void Update()
	{
		ReadDataFromStream();
		if(Input.GetKeyDown(KeyCode.C))
			Callibrate();
	}

	/// <summary>
	/// This reads all the data comming into the selected port.
	/// The data get's split and put into a local Float Array.
	/// This array is then used to assign the variables.
	/// </summary>
	private void ReadDataFromStream()
	{
		try
		{
			if(connectionOpened)
			{
				float[] str = Array.ConvertAll(stream.ReadLine().Split(';'), float.Parse);
				stream.BaseStream.Flush();
				rotation.x = -str[0] + offset.x;
				rotation.y = str[1] + offset.x;
				rotation.z = -str[2] + offset.z;
				gyroAccel = str[3];

				for(int i = 0; i < str.Length; i++)
					if(debugMode) Debug.Log(str[i]);
			}
		}
		catch(Exception)
		{
			Debug.Log("Something went wrong... Whoops :)");
		}
	}

	/// <summary>
	/// Opens an connection with the Arduino with the selected port.
	/// </summary>
	private void OpenConnection()
	{
		stream = new SerialPort(port, baudrate)
		{
			ReadTimeout = readTimeout
		};

		if(debugMode) Debug.Log("OpenConnection Started!");
		if(stream != null)
		{
			if(stream.IsOpen)
			{
				stream.Close();
				connectionOpened = false;
				if(debugMode) Debug.Log("Closing port, because it was already open!");
			}
			else
			{
				stream.Open();
				connectionOpened = true;
				if(debugMode) Debug.Log("Port Opened!");
			}
		}
		else
		{
			if(stream.IsOpen)
			{
				connectionOpened = true;
				if(debugMode) Debug.Log("Port is already open!");
			}
			else
			{
				connectionOpened = false;
				if(debugMode) Debug.Log("Port == null");
			}
		}
		if(debugMode) Debug.Log("Open Connection Finished Running!");
	}

	/// <summary>
	/// Closes the port when the application is closed. It's just the right thing to do.
	/// </summary>
	private void OnApplicationQuit()
	{
		if(stream != null)
		{
			stream.Close();
		}
	}

	/// <summary>
	/// This actually doesn't get used in the main game, but in the secundairy game that I dropped after some time.
	/// </summary>
	private void Callibrate()
	{
		float offsetX = -rotation.x;
		float offsetY = rotation.y;
		float offsetZ = -rotation.z;

		offset = new Vector3(offsetX, offsetY, offsetZ);
	}
}
