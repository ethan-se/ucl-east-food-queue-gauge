#include <Wire.h> 
#include <LiquidCrystal_I2C.h>

// Set the LCD address to 0x27 for a 16 chars and 2 line display
// If this doesn't work, try 0x3F
LiquidCrystal_I2C lcd(0x3F, 16, 2);

void setup()
{
  // 1. Manually start I2C on your specific pins
  // Syntax: Wire.begin(SDA_PIN, SCL_PIN);
  Wire.begin(D2, D3); 

  // 2. Initialize the LCD
  // The LCD library will now use the pins we just set above.
  lcd.init();

  // 3. Turn on the backlight and print
  lcd.backlight();
  
  lcd.setCursor(0,0); 
  lcd.print("Pins D2 & D3");
  
  lcd.setCursor(0,1); 
  lcd.print("Working!");
}

void loop()
{
  // Blink the backlight to show activity
  delay(1000);
  lcd.noBacklight();
  delay(500);
  lcd.backlight();
}