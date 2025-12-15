#define OPScafe_led D5
#define MSGcafe_led D6
#define MSGref_led D7

// the setup function runs once when you press reset or power the board
void setup() {
  // initialize digital pin LED_BUILTIN as an output.
  pinMode(OPScafe_led, OUTPUT);
  pinMode(MSGcafe_led, OUTPUT);
  pinMode(MSGref_led, OUTPUT);
}

// the loop function runs over and over again forever
void loop() {
  digitalWrite(OPScafe_led, HIGH);  // turn the LED on (HIGH is the voltage level)
  digitalWrite(MSGcafe_led, HIGH);  // turn the LED on (HIGH is the voltage level)
  digitalWrite(MSGref_led, HIGH);  // turn the LED on (HIGH is the voltage level)
  delay(1000);                      // wait for a second

  digitalWrite(OPScafe_led, LOW);   // turn the LED off by making the voltage LOW
  digitalWrite(MSGcafe_led, LOW);   // turn the LED off by making the voltage LOW
  digitalWrite(MSGref_led, LOW);   // turn the LED off by making the voltage LOW
  delay(1000);                      // wait for a second
}
