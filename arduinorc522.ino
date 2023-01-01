#include <MFRC522.h>
#include <SPI.h>



int RST_PIN = 9; //RC522 Define the module reset pin.
int SS_PIN = 10; //RC522 Define The module select pin

MFRC522 rfid(SS_PIN, RST_PIN); //Creating RC522 modul Settings
byte ID[4] = {
  33,
  247,
  8,
  35
}; //Define the Founder Card ID

void setup() {
  Serial.begin(9600); //Starting initiate serial communication.
  SPI.begin(); // Starting initiate SPI communication.
  rfid.PCD_Init(); //Starting RC522 Module
  
}

void loop() {


  if (!rfid.PICC_IsNewCardPresent()) //Wait reading new card ID
    return;

  if (!rfid.PICC_ReadCardSerial()) //If cant read Card ID , Waiting
    return;

  if (rfid.uid.uidByte[0] == ID[0] && //We compare the read card ID with the ID variable.
    rfid.uid.uidByte[1] == ID[1] &&
    rfid.uid.uidByte[2] == ID[2] &&
    rfid.uid.uidByte[3] == ID[3]) {
   

    ekranaYazdir();
    delay(100);
  } else {  //Commands inside are executed upon unauthorized access.
  
    ekranaYazdir();
   
  }
  rfid.PICC_HaltA();
}
void ekranaYazdir() {
  for (int sayac = 0; sayac < 4; sayac++) {
    Serial.print(rfid.uid.uidByte[sayac]);
    Serial.print("");
    
  }
  Serial.println("");
}
