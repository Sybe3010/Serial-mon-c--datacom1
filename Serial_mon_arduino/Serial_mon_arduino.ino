#define LED1 16
#define LED2 17

#define KNOP1 2
#define KNOP2 3

#define POT A1
#define SERVO 6

#define LCD_CS 9
#define LCD_DC 10
#define LCD_RST 21

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(LED1, OUTPUT);
  pinMode(LED2, OUTPUT);

  pinMode(POT, INPUT);

}
void loop() {
  // put your main code here, to run repeatedly:

} 
