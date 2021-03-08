#include <math.h>
#include <Chrono.h>
#include <Motor.h>
#include <Encoder.h>

#define PI 3.1415926535897932384626433832795

// Right motor ports
#define R_ENCODER_A 18
#define R_ENCODER_B 19
#define R_Blue 52
#define R_Dir 30
#define R_PWM 10

// Left motor ports
#define L_ENCODER_A 2
#define L_ENCODER_B 3
#define L_Blue 22
#define L_Dir 31
#define L_PWM 11

// Ultrasonic ports
#define Trigger 6
#define Echo 5
#define PIN_VOLT 7

// Control variables
double error = 0, previousError = 0, derivativeError = 0, integrativeError = 0;
const int deltaT = 50000;
const double kP = 280, kI = .0284, kD = 1800000, kRho = 1.2, kPP = 3;

// Wheels Variables
double wR = 0, wL = 0;

// Robot Variables in millimeters
const double diameter = 65, length = 180;
const double degreesPerMillimeter = 360.0 / (diameter * PI);
const double encoderTicksPerRev = 2370.72;
const double x0 = 0, y0 = 0, distance = 1000;
double v = 0, w = 0;

double centralDistance = 0, x = x0, y = y0, phi = 0;
int rightPwm = 0, leftPwm = 0, maxPwm = 100, minPwm = 40;

// Desired position Variables
double xd = distance + x0, yd = 0 + y0, phid = 0, rho; //atan2(yd - y, xd - x);

// Ultrasonic variables
long t, minDistanceUltrasonic = 30, d = minDistanceUltrasonic;
bool keepScanning = true;

// chrono variables
String content = "";
Chrono chronoPid;
Chrono chronoUltrasonic;
Chrono chronoRaspberryResponse;

// wheels motor and encode variables
Motor rightMotor(R_Dir, R_PWM, minPwm, maxPwm);
Motor leftMotor(L_Dir, L_PWM, minPwm, maxPwm);
Encoder rightEncoder(R_ENCODER_A, R_ENCODER_B, deltaT, encoderTicksPerRev);
Encoder leftEncoder(L_ENCODER_A, L_ENCODER_B, deltaT, encoderTicksPerRev);

void readRightEncoder()
{
  rightEncoder.updateCount();
}

void readLeftEncoder()
{
  leftEncoder.updateCount();
}

void setup()
{
  pinMode(R_Blue, OUTPUT);
  pinMode(R_ENCODER_A, INPUT);
  pinMode(R_ENCODER_B, INPUT);
  pinMode(R_Dir, OUTPUT);

  pinMode(L_Blue, OUTPUT);
  pinMode(L_ENCODER_A, INPUT);
  pinMode(L_ENCODER_B, INPUT);
  pinMode(L_Dir, OUTPUT);

  pinMode(Trigger, OUTPUT);
  pinMode(Echo, INPUT);
  pinMode(PIN_VOLT, OUTPUT);

  attachInterrupt(digitalPinToInterrupt(L_ENCODER_A), readLeftEncoder, CHANGE);
  attachInterrupt(digitalPinToInterrupt(R_ENCODER_A), readRightEncoder, CHANGE);
  Serial.begin(9600);
}

void loop()
{
  digitalWrite(R_Blue, HIGH);
  digitalWrite(L_Blue, HIGH);
  digitalWrite(PIN_VOLT, HIGH);

  if (chronoPid.hasPassed(50))
  {
    chronoPid.restart();
    wR = rightEncoder.getSpeed();
    wL = leftEncoder.getSpeed();
    updatePosition();

    phid = atan2(yd - y, xd - x);
    rho = sqrt(pow(xd - x, 2) + pow(yd - y, 2));
    error = phid - phi;
    error = atan2(sin(error), cos(error));
    derivativeError = (error - previousError) / deltaT;
    integrativeError += (kI * error);
    w = kP * error + kD * derivativeError + integrativeError + kPP * phid;
    v = kRho * rho;
    if (!(error > -PI / 2 && error <= PI / 2))
    {
      v *= -1;
    }

    rightPwm = (2 * v + (w * length)) / diameter;
    leftPwm = (2 * v - (w * length)) / diameter;

    if (abs(x - xd) <= 30 && abs(y - yd) <= 30)
    {
      rightMotor.setSpeed(0);
      leftMotor.setSpeed(0);
    }
    else if (d < minDistanceUltrasonic && keepScanning)
    {
      rightMotor.setSpeed(0);
      leftMotor.setSpeed(0);
      // send signal to raspberry
      Serial.println("Stopped");
      delay(1500);
      readSerialFromRasp();
    }
    else
    {
      rightMotor.setSpeed(rightPwm);
      leftMotor.setSpeed(leftPwm);
    }
    previousError = error;
  }
  if (chronoUltrasonic.hasPassed(200))
  {
    chronoUltrasonic.restart();
    triggerUltrasonic();
  }

  if (chronoRaspberryResponse.hasPassed(9000))
  {
    chronoRaspberryResponse.restart();
    if (!keepScanning)
    {
      keepScanning = true;
    }
  }
}

void updatePosition()
{
  double leftDegrees = -leftEncoder.getDistance();
  double rightDegrees = rightEncoder.getDistance();
  double leftDistance = leftDegrees / degreesPerMillimeter;
  double rightDistance = rightDegrees / degreesPerMillimeter;
  centralDistance = (leftDistance + rightDistance) / 2.0;
  phi += (rightDistance - leftDistance) / length;

  if (phi > 2.0 * PI)
  {
    phi -= 2.0 * PI;
  }
  else if (phi < 0.0)
  {
    phi += 2.0 * PI;
  }

  x -= centralDistance * cos(phi);
  y -= centralDistance * sin(phi);
}

void triggerUltrasonic()
{
  digitalWrite(Trigger, HIGH);
  delayMicroseconds(10);
  digitalWrite(Trigger, LOW);

  t = pulseIn(Echo, HIGH);
  d = t / 59;
}

void readSerialFromRasp()
{
  if (Serial.available())
  {
    content = Serial.readString();
    if (content == "ClosedRight")
    {
      goRight();
    }
    if (content == "ClosedLeft")
    {
      goLeft();
    }
    if (content == "UTurn")
    {
      goUTurn();
    }
    if (content == "RightLeft")
    {
      if (millis() % 2 == 0)
      {
        goRight();
      }
      else
      {
        goLeft();
      }
    }
    //"Nothing","Stop"
    content = "";
  }
}

void goRight()
{
  resetPosition();
  xd = 0 + x0;
  yd = distance + y0;
}

void goLeft()
{
  resetPosition();
  xd = 0 + x0;
  yd = -distance + y0;
}

void goUTurn()
{
  resetPosition();
  xd = -distance + x0;
  yd = 0 + y0;
}

void goForward()
{
  resetPosition();
  xd = distance + x0;
  yd = 0 + y0;
}

void resetPosition()
{
  x = x0;
  y = y0;
  phi = 0;
  keepScanning = false;
}
