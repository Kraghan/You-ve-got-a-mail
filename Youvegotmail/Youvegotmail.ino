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
  unsigned long currentTime = millis();
  float elapsed = ((float)currentTime - lastTime) / 1000.f;
  lastTime = currentTime;
  accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  float movementAngle = float(gz)*elapsed/131.f;
  
  if(fabs(movementAngle) > 0.02f)
    handleBarPosition = handleBarPosition + movementAngle;

  float magnetoVal = magnetometer.read();
  bool active = false;
  if(abs(magnetoVal - 512) >= 2)
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


