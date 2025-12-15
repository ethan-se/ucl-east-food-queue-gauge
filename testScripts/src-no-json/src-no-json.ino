// NodeMCU code for UCL East Food Gauge: "Gauge N'Grab"

// Modified code, based on sketch by digitalurban:
// https://github.com/ucl-casa-ce/Open-Gauges/blob/main/Arduino/WindSpeedGauge/WindSpeedGauge.ino


#include <ServoEasing.hpp>
#include <WiFiClient.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <NTPClient.h>
#include <WiFiUdp.h>
#include <Wire.h> 
#include <LiquidCrystal_I2C.h>
#include "arduino_secrets.h"

// 180 degree servo, an SG90MG is recomended. Example code is for WindSpeed
// using an mqtt feed.
// It uses the 2.14:1 gear train
// The code can bbe adaopted for other servo types


// Set Servo to the Servo Easing Library

ServoEasing servo;

// set up LCD display properties

LiquidCrystal_I2C lcd(0x3F, 16, 2); // 16 characters, 2 lines

// setup angle for MG90S
int angle;

// setup LED pins

#define OPScafe_led D5
#define MSGcafe_led D6
#define MSGref_led D7

const int buttonPin1 = D0;  // the number of the pushbutton pin
const int buttonPin2 = D1; 
const int buttonPin3 = D8; 

// set up MQTT topic selection arrays
const char* mqttTopics[] = {
  "student/CASA0019/gauge&grab/pool_street/queue_time",
  "student/CASA0019/gauge&grab/marshgate_cafe/queue_time",
  "student/CASA0019/gauge&grab/marshgate_canteen/queue_time"
};

// Calculate the number of topics automatically
// (Total size of array / size of one entry)
const int topicCount = sizeof(mqttTopics) / sizeof(mqttTopics[0]);
int roomSelIndex = 0; //controls what topic is subscribed and LEDs lit
// connect to wifi and mqtt server

const char* ssid = SECRET_SSID;
const char* password =  SECRET_PASS;
const char* mqttServer = "mqtt.cetools.org";  //Edit this for your own MQTT or leave for the CE Wind Speed Feed
const int mqttPort = 1883;

WiFiClient gaugeNgrab;
PubSubClient client(gaugeNgrab);



void setup()
{
  Serial.begin(9600);

  //initialise LCD display
  Wire.begin(D2, D3); 
  lcd.init();

  //turn on LCD backlight
  lcd.backlight();

  //initialise buttons as inputs
  pinMode(buttonPin1, INPUT); //OPS cafe
  pinMode(buttonPin2, INPUT); //Marshgate cafe
  pinMode(buttonPin3, INPUT); //Marshgate refectory

  pinMode(OPScafe_led, OUTPUT);
  pinMode(MSGcafe_led, OUTPUT);
  pinMode(MSGref_led, OUTPUT);

  //turn on all LEDs at startup
  digitalWrite(OPScafe_led, HIGH);
  digitalWrite(MSGcafe_led, HIGH);
  digitalWrite(MSGref_led, HIGH);

  //Notify user of servo calibration
  lcd.setCursor(0,0); 
  lcd.print("Calibrating");
  
  lcd.setCursor(0,1); 
  lcd.print("servo...");

  //MQTT connection code
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
  Serial.print("Moving to 180");
  servo.easeTo(105); // determined via trial and error
 
  delay (1000);
  
  Serial.print("Moving to 0");
  servo.easeTo(10);  // Edit Values to Match Your Servo
 
  Serial.print("Pausing for 5 seconds");
  delay (5000);

  //Notify user that MQTT connection is in progress
  lcd.setCursor(0,0); 
  lcd.clear();           // Wipes the screen clean
  lcd.print("Connecting");
  
  lcd.setCursor(0,1); 
  lcd.print("via MQTT...");

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
    if(client.connect("gaugeNgrab")) 
{
     Serial.println("connected");
     
    }else{
      Serial.print("failed state ");
      Serial.print(client.state());
      delay(2000);
    }
  }
 
  client.subscribe(mqttTopics[roomSelIndex]);
  //set LEDs to index 0 configuration (OPS Cafe)
  digitalWrite(OPScafe_led, HIGH);
  digitalWrite(MSGcafe_led, LOW); 
  digitalWrite(MSGref_led, LOW); 
  // Tell user what meal of the day is for OPS (to initialise)
  lcd.clear();           // Wipes the screen clean
  lcd.setCursor(0,0); 
  lcd.print("Meal of the day:");
  
  lcd.setCursor(0,1); 
  lcd.print("Chicken: 3.95");
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
      client.subscribe(mqttTopics[roomSelIndex]);
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void changeTopic(int newIndex) {
  // 1. Unsubscribe from the OLD topic
  client.unsubscribe(mqttTopics[roomSelIndex]);
  Serial.print("Unsubscribed from: ");
  Serial.println(mqttTopics[roomSelIndex]);

  // 2. Update the index
  roomSelIndex = newIndex;

  // 3. Subscribe to the NEW topic
  client.subscribe(mqttTopics[roomSelIndex]);
  Serial.print("Subscribed to: ");
  Serial.println(mqttTopics[roomSelIndex]);
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
  String q = String((char*)payload);
  int qt = q.toInt();
  int queueTime = constrain(qt, 0, 20); //failsafe to prevent pointer going past 180deg
  
  // Edit the range to match your own servo configeration  
  angle = map(queueTime, 0, 20, 10, 105);   // Edit Values to Match Your Data Values and Servo Range
 

 
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
  Serial.print("Queue Time ");
  Serial.println(queueTime);
  delay(500); // Allow transit time
}

void loop() {
  // 1. Ensure we are still connected to MQTT
    if (!client.connected()) {
      reconnect();
    }
    // 2. Process MQTT messages (Check for new data)
    client.loop();
  

  // read the state of the pushbutton value:
  if (digitalRead(buttonPin1) == HIGH && roomSelIndex != 0) {
     Serial.println("Button 1 Pressed: Switching to OPS Cafe");
     changeTopic(0);
     //set LEDs to new selection
     digitalWrite(OPScafe_led, HIGH);
     digitalWrite(MSGcafe_led, LOW); 
     digitalWrite(MSGref_led, LOW); 
     delay(200); // Small debounce delay
     //display meal of day
     lcd.clear();           // Wipes the screen clean
     lcd.setCursor(0,0); 
     lcd.print("Meal of the day:");
      
     lcd.setCursor(0,1); 
     lcd.print("Chicken: 3.95");
  }
  else if (digitalRead(buttonPin2) == HIGH && roomSelIndex != 1) {
     Serial.println("Button 2 Pressed: Switching to Marshgate Cafe");
     changeTopic(1);
     //set LEDs to new selection
     digitalWrite(OPScafe_led, LOW);
     digitalWrite(MSGcafe_led, HIGH); 
     digitalWrite(MSGref_led, LOW); 
     delay(200); 
     //display meal of day
     lcd.clear();           // Wipes the screen clean
     lcd.setCursor(0,0); 
     lcd.print("Meal of the day:");
      
     lcd.setCursor(0,1); 
     lcd.print("Beef: 4.10");
  }
  else if (digitalRead(buttonPin3) == HIGH && roomSelIndex != 2) {
     Serial.println("Button 3 Pressed: Switching to Marshgate Refectory");
     changeTopic(2);
     //set LEDs to new selection
     digitalWrite(OPScafe_led, LOW);
     digitalWrite(MSGcafe_led, LOW); 
     digitalWrite(MSGref_led, HIGH); 
     delay(200);
     //display meal of day
     lcd.clear();           // Wipes the screen clean
     lcd.setCursor(0,0); 
     lcd.print("Meal of the day:");
      
     lcd.setCursor(0,1); 
     lcd.print("Lamb: 3.55");
    }
  
}