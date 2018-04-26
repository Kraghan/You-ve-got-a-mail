#include <SoftwareSerial.h>
#include <TinkerKit.h>
#include <Wire.h>
#include "I2Cdev.h"
#include "MPU6050.h"
TKHallSensor magnetometer(I1);

float handleBarPosition = 0;

unsigned long lastCall = 0;

int numberOfActivation = 0;

bool inFrontOf = false;

MPU6050 accelgyro;
 
int16_t ax, ay, az;
int16_t gx, gy, gz;

unsigned long lastTime = 0;
float calibrationValue = 0;
void setup() 
{  
  Wire.begin(); 
  Serial.begin(9600);
  while (!Serial);
  accelgyro.initialize();

  lastTime = millis();
  Serial.setTimeout(5);
}

void loop () 
{
  
  String data = Serial.readString();
  if(data == "Calibrate")
  {
    handleBarPosition = 0;
  }

  
  unsigned long currentTime = millis();
  float elapsed = ((float)currentTime - lastTime) / 1000.f;
  lastTime = currentTime;
  accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  handleBarPosition = 0.98*(handleBarPosition+float(gz)*elapsed/131) + 0.02*atan2((double)ax,(double)az)*180/PI;

  //Serial.println(handleBarPosition);

  float magnetoVal = magnetometer.read();
  bool active = false;
  if(abs(magnetoVal - 510) >= 5)
  {
    if(!inFrontOf)
      active = true; 
    inFrontOf = true;
  }
  else
    inFrontOf = false;
  String str = "";
  str += handleBarPosition - calibrationValue;
  str += ";";
  str += active;
  Serial.println(str);
}


