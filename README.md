# Gauge and Go
"Gauge and Go" is a group project submission for CASA0019, Sensor Data Visualisation, as part of a Masters course at CASA, University College London.

Gauge and Go is a physical device that displays a simulated data feed of queue lengths at UCL East's three food venues in real time. It is accompanied by a digital twin: a 3D model contained in an AR smartphone app, displaying further details about queue length and food options.

This repository contains:
-  `/data/` - files used to create a dummy data stream of queuing and menu information, sent via MQTT.
- `/digitalTwin/` - 3D models, Unity project files and `.apk` file to run the digital twin device on a smartphone.
- `/docs/`: stores media embedded within this readme.
- `/physicalDevice/` - files used to reproduce the enclosure of the physical device.
- `/planning/` - planning documents written to assist in defining project scope and allocating tasks to each team member. 
- `/src/` - the source code written for the NodeMCU powering the physical device.
- `/testScripts/` - Arduino scripts written to debug seperate components of the physical device. 

The following resources may also be of interest:
- Digital information leaflet (used for project submission) hosted on GitHub Pages: https://ethan-se.github.io/ucl-east-food-queue-gauge/
- Demonstration of digital twin AR app available on YouTube: https://youtube.com/shorts/w2yIuAzpJI0

## Rationale
Gauge and Go was motivated by the spatial monopoly enjoyed by the three [Food at UCL](https://www.foodatucl.com/locations/) venues on UCL East campus: Marshgate Cafe, Marshgate Refectory and Pool Street Cafe. Nearby alternatives at comparable price points require significantly more travel time. Leaving Queen Elizabeth Olympic Park to access cheaper food venues on Stratford High Street comes at the cost of increased pollution, noise and road crossings. These factors contribute to psychological barriers which serve to geographically isolate the campus from competing food outlets (Anciaes, Jones and Mindell, 2016), encouraging Food at UCL venues to be the primary choice for students and staff. 

<!-- The nearest food venue in Queen Elizabeth Olympic Park at the same price point, [*Pret a Manger* on Endeavour Square](https://maps.app.goo.gl/fiWMevSzQXqHyPjE8), requires a 20 minute round-trip. A nearby Tesco Express requires a more reasonable 12-minute round-trip for cheaper food, at the cost of increased noise, road crossings and noise. -->

Given the prominence of Food at UCL venues, our project was built to improve the experience of a significant proportion of students and staff, who may choose to at these venues. By creating a real-time display of estimated queue times, the project allows users to choose a more efficient time to get their lunch. By highlighting special deals in the physical environment, this raises awareness of low-cost menu options. This information would normally be accessible through posters and screens in the venues themselves, or through downloading the Food at UCL smartphone app. 