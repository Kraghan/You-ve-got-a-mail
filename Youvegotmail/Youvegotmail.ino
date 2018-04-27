#include <SoftwareSerial.h>
#include <TinkerKit.h>
#include <SerialCommand.h>
#include <Wire.h>
#include "I2Cdev.h"
#include "MPU6050.h"
TKHallSensor magnetometer(I1);
SerialCommand serialCommand;

float handleBarPosition = 0;

unsigned long lastCall = 0;
unsigned long lastTime = 0;
int numberOfActivation = 0;

bool inFrontOf = false;

MPU6050 accelgyro;

int16_t ax, ay, az;
int16_t gx, gy, gz;

void setup() 
{  
  Serial.begin(9600);
  while (!Serial);
  accelgyro.initialize();
  serialCommand.addCommand("PING", pingHandler);
  serialCommand.addCommand("GET_HANDLEBAR", handlebarHandler);
  serialCommand.addCommand("GET_WHEELSPEED", wheelSpeedHandler);
}

void loop () 
{
 unsigned long currentTime = millis();
 float elapsed = ((float)currentTime - lastTime) / 1000.f;
 lastTime = currentTime;
 accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
 float movementAngle = float(gz)*elapsed/131.f;

 //if(fabs(movementAngle) > 0.02f)
   handleBarPosition = handleBarPosition + movementAngle;
  
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
  Serial.println(handleBarPosition);
}

void wheelSpeedHandler()
{
  
  unsigned long current = millis();
  Serial.println(numberOfActivation);
  lastCall = current;
  numberOfActivation = 0;
}

