/** sendDataToArduino.ino  loopBack y manipula un IO digital. 
 *  
 *  Descripción.
 *      Lee y escribe un dato desde el puerto serial com.
 *      El dato recibido es filtrado y procesado para habilitar-desabilitar una
 *      salida digital conforme a la Tabla PinOut.
 *      
 *      PinOut
 *      IO    Descripción
 *      13    led color ambar.  Si recibe token <IO13>  enciende en alto
 *       
 * Hardware.      
 *        
 *       PB5/IO13 ---1k Ohm---ledYellow---0V.
 *       
 *       avrdude: Version 6.3-20190619
 *        Copyright (c) 2000-2005 Brian Dean, http://www.bdmicro.com/
 *        Copyright (c) 2007-2014 Joerg Wunsch
 *
 *        Using Port                    : COM13
 *        Using Programmer              : arduino
 *        Overriding Baud Rate          : 115200
 *        AVR Part                      : ATmega328P
 *      
 *       fuente:
 *       -Sepulvéda-Cervantes G. EdissaBoy. Configuración y protocolo comunicaciones seriales. 
 *       -Astoraque Duran J. jadsa tv,
 *       UNITY - ARDUINO #1: COMUNICACION SERIAL UNITY ARDUINO - ENVIAR DATOS A ARDUINO
 *       Revisado el 02Abril21. Disponible en:
 *       https://www.youtube.com/watch?v=p7qtEKvKG5I
 *       
 *       String()
 *       https://www.arduino.cc/reference/en/language/variables/data-types/stringobject/
 */
 
#define velocidadSincrona 57600         // declara
      
const uint8_t ledAmbar = 13;            // pinOut
bool yellowState = false;               // estado inicial de recepción
String inputData =  "IO13";             // token recepción 
String outputData = "io13";             // token transmisión 
const  unsigned int baudRate = 57600;   // bit rate 
String inputString = "";                // recibe y registra dato desde puerto
bool   stringComplete = false;          // señalizador de recepción
unsigned int sizeBufferRecMemory = 20;  // define tamaño del buffer de recepción
unsigned char EOS = '\n';               // señalizador de dato recibido EndOfString

// declaración de funciones prototipo
void serialEvent();
void processCmd(String str);

void setup()
{
  // Serial.begin(velocidadSincrona);         // 17.36us-bit
  Serial.begin(baudRate);                     // 17.36us-bit
  while (!Serial) {;}                         // wait for serial port to connect. Needed for native USB port only
  inputString.reserve(sizeBufferRecMemory);   // dimensiona buffer de recepción
  inputString = "";                           // inicializa buffer de recepción
  stringComplete = false;                     // señalizador de recepción
  pinMode(ledAmbar, OUTPUT);                  // IO13 como salida digital
  digitalWrite(ledAmbar,  LOW);               // IO13<=0
  Serial.print("0k\n");                       // envia inicializado
}

void loop() 
{
  
  if(stringComplete) 
  {
    //Serial.print("CMD received: ");
    //Serial.println(inputString); // muestra el buffer de recepción
    inputString.trim();          // remueve espacios en blanco al pricipio-final
    processCmd(inputString);     // actualiza estado boleano en IO led 
    inputString = "";            // inicializa buffer de recepción
    stringComplete = false;      // señalizador de recepción
   }
   
}
/* actualiza buffer de recepción hasta encontrar EndOfSerial */
void serialEvent()
  {
    while(Serial.available())
      {
        char inChar = (char)Serial.read();           // recibe dato RxD<=payLoad
        if (inChar == EOS){ stringComplete = true; } // habilita el acceso 
        else { inputString += inChar; }              // actualiza buffer de receoción
      }
  }

/* actualiza estado boleano en IO13 led */  
void processCmd(String str)
  {
  if (str.equals("IO13")) 
      { 
        yellowState = !yellowState;             // conmuta estado
        digitalWrite(ledAmbar,yellowState);     // actualiza IO13 
        delay(50);
        Serial.println(yellowState);               // envia señalizador outputData
        
      }
  }
