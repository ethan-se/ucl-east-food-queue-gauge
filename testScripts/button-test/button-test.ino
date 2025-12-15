// Button test for selecting between MQTT topics for each food location

const int buttonPin1 = D0;  // the number of the pushbutton pin
const int buttonPin2 = D1; 
const int buttonPin3 = D8; 

// variables will change:
int buttonState1 = 0;  // variable for reading the pushbutton status
int buttonState2 = 0;  // variable for reading the pushbutton status
int buttonState3 = 0;  // variable for reading the pushbutton status


void setup() {
  // initialize serial comms
  Serial.begin(9600);
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin1, INPUT);
  pinMode(buttonPin2, INPUT);
  pinMode(buttonPin3, INPUT);
}

void loop() {
  // read the state of the pushbutton value:
  buttonState1 = digitalRead(buttonPin1);
  buttonState2 = digitalRead(buttonPin2);
  buttonState3 = digitalRead(buttonPin3);

  // check if the pushbutton is pressed. If it is, the buttonState is HIGH:
  if (buttonState1 == HIGH) {
    // pretend to turn LED on:
    Serial.println("OPS Cafe selected");
  } else {
    // do nothing
  }
  if (buttonState2 == HIGH) {
    // pretend to turn LED on:
    Serial.println("Marshgate Cafe selected");
  } else {
    // do nothing
  }
  if (buttonState3 == HIGH) {
    // pretend to turn LED on:
    Serial.println("Marshgate refectory selected");
  } else {
    // do nothing
  }
}
