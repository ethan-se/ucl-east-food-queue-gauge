// #include <Servo.h> //import library
#include <ServoEasing.hpp> //import library

ServoEasing myservo;     //initialise servo object
void setup() {
  Serial.begin(9600);
  myservo.attach(D4); //set pin D4 to control servo. REMEMBER: D4 !== 4
  Serial.println("setup complete");
}

void loop() {
  Serial.println("init loop");


  for (int pos = 0; pos<=180; pos++) { //iterate over 0-180 degrees of servo motion:
    myservo.write(pos); //update servo position every degree
    delay(15);
  }
  Serial.println("servo moved");
  delay(1000);

  for (int pos = 180; pos >=0; pos--) {
    myservo.write(pos);
    delay(15);
  }
  Serial.println("servo moved back");
  delay(1000);
}
