# Autonomous robot using MLP

This project provides the different solutions needed to build a autonomous differential mobile robot and making decisions from taking a picture of street signs and processing it on a MLP (multilayer perceptron).

The system is divided by three layers for efficiency and improve reaction response.

## The layers are:

- Arduino MEGA 2560
  - In charge of the robot position and PID control.
  - Slave of the Raspberry.
- Raspberry PI 3 B+
  - In charge of taking the photos and sending it to the MLP API using Python.
  - Slave of the web server.
- Web server
  - Web API written in .NET Core 3.
  - Can create neural networks and will save it into a SQL Server using Entity Framework.
  - For Training process, it will use a custom matlab library.
  - Documented with swagger
  - Integrated with a WPF client to create image sets for the neural networks.

---

## Dependencies:
