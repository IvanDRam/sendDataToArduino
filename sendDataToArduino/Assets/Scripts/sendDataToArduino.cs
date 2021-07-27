/** sendDataToArduino.cs  Interface loopBack. Envía-recibe datos vía puerto serial y administra
un IO digital en un microcontrolador.

Descripción.  

Se presenta una interface 2D con un boton que al ser presionados onClick()
envia vía puerto seria una cadena de caracteres.

    |-----------|
    |  ON/OFF   |---payLoad--->comn--IO13--ledEmpotrado---0V     
    |-----------|  

  Dato     PinOut   Microcontrolador
 enviado           
  ON/OFF    13    ledEmpotrado.  Si recibe etiqueta io13  enciende en alto led empotrado


  1] Crear un GameObject => Renombrar a SerialObj => crear y adjuntar Script "sendDataToArduino.cs"

            - Procedimiento para adjuntar SerialObj al Btn_ON_OFF_IO13         
  
  1] Canvas
       |¬Btn_ON_OFF_IO13, click (+)
           |¬OnClick() <= adjuntar SerialObj en el campo: Select Object          
  2] En el campo No Function seleccionar: sendDataToArduino/sendPayLoad(string)
  3] Teclear el argumento "IO13" para sendPayLoad

            - Procedimiento para adjuntar Script al Btn_ClosePort         
  1] Canvas
       |¬Btn_ClosePort, click (+)
           |¬OnClick() <= adjuntar SerialObj en el campo: Select Object          
  2] En el campo No Function seleccionar: sendDataToArduino/closePort()
  
 Componente Arduino
 sendDataToArduino.ino  lee puerto serial y manipula IO digitales 
 Fuente:

 https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport.rtsenable?view=dotnet-plat-ext-5.0 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO.Ports; // Edit/Project Setting.../Player/Configuration/Api Compatibility Lever*  /.Net4.x Equivalent

public class sendDataToArduino : MonoBehaviour
{
    SerialPort serialPort;                  // instancia de un puerto de communiciación
    public string portName;                 // COMM-Arduino
    public string dataInput = " ";          // dataInput<=RxD
    [Range(0,100)]  // decorador 
    public int x;
    [ContextMenu("IncrementaX")]  // decorador 
    public void IncX()
    {
        x++;
    }
    /*configura y abre puerto comunicaciones */
    void Start()
    {
        ConfigSerialPort();                 // configuración puerto COMM-Unity
        serialPort.Open();                  // abre puerto de comunicaciones 
        Debug.Log("Serial port: O P E N "); // monitoriza estado del puerto
    }
    /* cierra puerto, cuando una escena-juego finaliza*/
    void OnDestroy()  // 
    {
        ClosePort();        // cierra puerto
    }

    /* retorna y monitoriza dataInput en interface contexMenu*/
    [ContextMenu("FuncionDecarador")]  // decorador 
    public string ReadLine()
    {
        dataInput = serialPort.ReadLine();    // dataInput <= Rxd-Arduino
        return dataInput;
    }

    /* configura parámetros de Txd,Rxd COMM-Unity */
    private void ConfigSerialPort()
    {
        serialPort = new SerialPort(portName,          // COMM-Arduino
                                        57600,         // velocidad de Txd-Rxd
                                        Parity.None,   // sin bit paridad
                                        8,             // bits trama de datos
                                        StopBits.One); // instancia de puerto serial
        serialPort.Handshake = Handshake.None;
        serialPort.RtsEnable = true;
        serialPort.ReadTimeout = 200;    //sets the number of milliseconds before a time-out occurs when a read operation does not finish.
        serialPort.WriteTimeout = 200;
    }

    /*loopback de un token. Envía-Recibe entre Unity-Arduino */
    public void SetPayLoad(string payload)
    {
        serialPort.WriteLine(payload);          // transmite token
        Debug.Log("Dato enviado: " + payload);  // monitor
        print("Dato recibido: " + ReadLine());  // recibe token
    }

    /* recibe e imprime en consola un mensaje del serialPort */
    public void GetPayLoad()
    {
        Debug.Log("Read input data: " + serialPort.ReadLine()); // imprime en consola
    }

    /*libera puerto serial*/
    public void ClosePort()
    {
        serialPort.Close();       // cierra puerto serial
        Debug.Log("Serial port: C L O S E ");
    }
}
