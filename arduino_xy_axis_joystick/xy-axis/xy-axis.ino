const int SW = 2; 
const int VRx = A0; 
const int VRy = A1; 

void setup() {
  Serial.begin(115200);
  pinMode(SW, INPUT);
  digitalWrite(SW, HIGH);
}
void loop() {
 if (!digitalRead(SW))
  {
    Serial.println("Push button pressed!");
  }
  if (analogRead(VRx)>=850) 
  {
    Serial.write(6);
    Serial.flush();
    delay(20);
  }
  else if (analogRead(VRx)<= 150)
  {
    Serial.write(4);
    Serial.flush();
    delay(20);
  }
  else if (analogRead(VRy)<= 150)
  {
    Serial.write(8);
    Serial.flush();
    delay(20);
  }
  else if (analogRead(VRy)>= 850)
  {
    Serial.write(2);
    Serial.flush();
    delay(20);
  }
  else
  {
    Serial.write(0);
    Serial.flush();
    delay(20);
  }
}
