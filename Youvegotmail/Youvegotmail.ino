#include <SoftwareSerial.h>
#include <TinkerKit.h>
#include <SerialCommand.h>
TKHallSensor magnetometer(I1);
SerialCommand serialCommand;

int potentiometterPin = 0;

float handleBarPosition = 0;

unsigned long lastCall = 0;

int numberOfActivation = 0;

bool inFrontOf = false;

void setup() 
{  
  Serial.begin(9600);
  while (!Serial);

  serialCommand.addCommand("PING", pingHandler);
  serialCommand.addCommand("GET_HANDLEBAR", handlebarHandler);
  serialCommand.addCommand("GET_WHEELSPEED", wheelSpeedHandler);
}

void loop () 
{
  handleBarPosition = analogRead(potentiometterPin);
  
  float magnetoVal = magnetometer.read();

  if(abs(magnetoVal - 512) >= 5)
  {
    if(!inFrontOf)
      numberOfActivation++; 
    inFrontOf = true;
  }
  else
    inFrontOf = false;
  
  if (Serial.available() > 0)
    serialCommand.readSerial();
}

void pingHandler (const char *command) {
 Serial.println("PONG");
}

void handlebarHandler()
{
  //Serial.println(handleBarPosition / 1023.0);
  Serial.println(511);
}

void wheelSpeedHandler()
{
  
  unsigned long current = millis();
  Serial.println(numberOfActivation);
  lastCall = current;
  numberOfActivation = 0;
}

