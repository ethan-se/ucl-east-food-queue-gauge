// NodeMCU code for UCL East Food Gauge: "Gauge N'Grab"

// Modified code, based on sketch by digitalurban:
// https://github.com/ucl-casa-ce/Open-Gauges/blob/main/Arduino/WindSpeedGauge/WindSpeedGauge.ino


#include <ServoEasing.hpp>
#include <WiFiClient.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <NTPClient.h>
#include <WiFiUdp.h>
#include "arduino_secrets.h"

// 180 degree servo, an SG90MG is recomended. Example code is for WindSpeed
// using an mqtt feed.
// It uses the 2:14 gear train
// The code can bbe adaopted for other servo types


// Set Servo to the Servo Easing Library

ServoEasing servo;
//Servo servo;
// create an instance of the servo class

int angle;

// set up night mode deep sleep


// connect to wifi and mqtt server

const char* ssid = SECRET_SSID;
const char* password =  SECRET_PASS;
const char* mqttServer = "mqtt.cetools.org";  //Edit this for your own MQTT or leave for the CE Wind Speed Feed
const int mqttPort = 1883;

WiFiClient espWindSpeedLecture23;
PubSubClient client(espWindSpeedLecture23);



void setup()


{
  Serial.begin(9600);

  WiFi.mode(WIFI_STA);
  WiFi.softAPdisconnect(true);

  servo.write(3); 
  servo.attach(D4);

// Servo Sweep - Useful for Calibration - Edit for own Servo/Dial Range
  servo.setEasingType(EASE_LINEAR);
  servo.setSpeed(15);
  Serial.print("Moving to 0");
  servo.easeTo(10);
  
  delay (1000);
  Serial.print("Moving to 270");
  servo.easeTo(140); // Edit Values to Match Your Servo
  // would be 86.66 degrees?
 
  delay (1000);
  
  Serial.print("Moving to 0");
  servo.easeTo(10);  // Edit Values to Match Your Servo
 
  Serial.print("Pausing for 5 seconds");
  delay (5000);


  WiFi.mode(WIFI_STA);
  WiFi.softAPdisconnect(true);
  
  WiFi.setSleepMode(WIFI_NONE_SLEEP);
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Connecting to WiFi..");
  }
  Serial.println("Connected to the WiFi network");
  client.setServer(mqttServer, mqttPort);
  
  client.setCallback(callback);
  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
    if(client.connect("espWindSpeedLecture23")) 
{
     Serial.println("connected");
     
    }else{
      Serial.print("failed state ");
      Serial.print(client.state());
      delay(2000);
    }
  }
 
  client.subscribe("personal/ucfnaps/downhamweather/windSpeed_mph");
}
//Reconnect if connection lost
void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (client.connect(clientId.c_str())) {
      Serial.println("connected");
      // Once connected, publish an announcement...
     
      // ... and resubscribe
      client.subscribe("personal/ucfnaps/downhamweather/windSpeed_mph");
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}



void callback(const char* topic, byte* payload, unsigned int length) {
  
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  for (int i=0;i<length;i++) {
    Serial.print((char)payload[i]);
   
  }
  
  Serial.println();

//Convert payload to a string and then to an int to negate error reading bytes
  
  payload[length] = '\0';
  String w = String((char*)payload);
  int wind = w.toInt();
  
  // Edit the range to match your own servo configeration  
  angle = map(wind, 0, 40, 10, 140);   // Edit Values to Match Your Data Values and Servo Range
 

 
  // Calculate the difference in angle
  int angleDifference = abs(servo.read() - angle);

  // Adjust speed and easing type based on the angle difference
  if (angleDifference < 10) {
    servo.setSpeed(5); // Slower for small changes
    servo.setEasingType(EASE_LINEAR); // Linear easing for smoother small movements
  } else {
    servo.setSpeed(10); // Faster for larger changes
    servo.setEasingType(EASE_CUBIC_IN_OUT); // Cubic easing for larger movements
  }

  // Move servo to the new position
  servo.easeTo(angle);
  Serial.print("Wind Speed ");
  Serial.println(wind);
  delay(500); // Allow transit time
}

void loop()

{
 
  // and now wait for 1s, regularly calling the mqtt loop function
  for (int i=0; i<1000; i++) {
    client.loop();
    delay(1);
  }
  
   }