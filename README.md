# Gauge and Go
"Gauge and Go" is a group project submission for CASA0019, Sensor Data Visualisation, as part of a Masters course at CASA, University College London.

Gauge and Go is a physical device that displays a simulated data feed of queue lengths at UCL East's three food venues in real time. It is accompanied by a digital twin: a 3D model contained in an AR smartphone app, displaying further details about queue length and food options.

This repository contains:
-  `/data/` - files used to create a dummy data stream of queuing and menu information, sent via MQTT.
- `/digitalTwin/` - 3D models, Unity project files and `.apk` file to run the digital twin device on a smartphone.
- `/docs/` - stores media embedded within this readme.
- `/physicalDevice/` - files used to reproduce the enclosure of the physical device.
- `/planning/` - planning documents written to assist in defining project scope and allocating tasks to each team member. 
- `/src/` - the source code written for the NodeMCU powering the physical device.
- `/testScripts/` - Arduino scripts written to debug seperate components of the physical device. 

The following resources may also be of interest:
- Digital information leaflet (used for project submission) hosted on GitHub Pages: https://ethan-se.github.io/ucl-east-food-queue-gauge/
- Demonstration of digital twin AR app available on YouTube: https://youtube.com/shorts/w2yIuAzpJI0