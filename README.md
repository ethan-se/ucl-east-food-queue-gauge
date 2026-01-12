# Gauge & Grab

## Project Authors

This project was collaboratively developed by:

Ethan Taylor

Madina Diallo

Yussr Osman Kamil Bashir

🔗 GitHub Repository

The full source code, documentation, and project assets are available at:

👉 https://github.com/ethan-se/ucl-east-food-queue-gauge

## 📄 About This Repository
This repository contains the code, documentation, and supporting materials for the UCL East Food Queue Gauge project, developed as part of an academic group assignment.


**A Physical-[Digital](https://youtube.com/shorts/w2yIuAzpJI0?si=osPyPwr-uZf5rIN4) IoT System for Campus Dining Decisions**

![Picture of Gauge and Grab physical device](./docs/physicalGauge-cropped.jpg)

---
## 1. Introduction & Rationale

University campus users frequently face uncertainty when deciding where to eat, particularly during peak hours. At UCL East, dining locations such as Pool Street Café, Marshgate Café, and Marshgate Canteen offer varied menus, daily specials, prices, and service speeds. However, users currently lack a quick, data-driven way to compare queue lengths and menu value in real time. This often leads to inefficiencies, long queues, and dissatisfaction.

Gauge & Grab was developed to address this everyday decision-making challenge through the design of a physical data device paired with a digital twin interface. The project explores how live (simulated) data, delivered through an Internet of Things (IoT) pipeline, can be visualised both analogue-physically and digitally augmented to support rapid, informed choices.

(Letsa, 2017)

The system allows users to instantly view:
- Estimated queue waiting time
- Daily specials and prices (Monday–Friday)
- Location-specific service characteristics
  

**Research Question:**  
*How can food-service data be presented in a fast, simple, and meaningful way to support informed decision-making on campus?*

---

## 2. Project Aims & Group Objectives

As a group, our objectives were to:
- Design and prototype a physical gauge that visualises live queue time immediately.
- Develop a digital twin and dashboard (Unity-based) that responds to the same live data feed.
- Use MQTT to demonstrate a real-time, end-to-end data pipeline.
- Enable users to make a dining decision within 5 seconds of interacting with the device.
- Show how analogue and digital systems can work together in a smart campus context.

---

## 3. Physical Device Design

### a. Aesthetic & Interaction Design

The physical gauge was designed to integrate naturally into the built environment at UCL East. Its form factor was inspired by devices commonly found near lecture rooms, such as thermostats or card readers, encouraging intuitive, brief interactions upon entering or exiting a space. 

(Aman, 2025)

The enclosure features:
- A laser-cut black acrylic front panel
- White etched typography using Helvetica, chosen for legibility and familiarity (Osborn, 2012)
- Magnetic mounting inspired by prior Connected Environments projects (e.g. SubRadar) (Low et al., 2024)
- A minimalist black-and-white aesthetic to complement existing campus infrastructure.
  
<img width="244" height="136" alt="image" src="https://github.com/user-attachments/assets/11847f94-c41a-4187-ab09-2ced723d528c" />



Each dining location is represented by:
- A dedicated button
- A corresponding LED indicator
- Clear etched labels on the front panel
  
  <img width="343" height="172" alt="image" src="https://github.com/user-attachments/assets/4bb741b6-09ef-4585-8646-79e2517a47fc" />


This allows users to switch between locations with a single press, immediately updating the data displayed.

### b. Dial Graphic & Queue Representation

Queue time is visualised through a servo-driven pointer rotating across a semi-circular dial. The dial uses colour-coded zones:
- **Green (0–5 min)** – acceptable wait
- **Yellow (5–10 min)** – situational
- **Red (10+ min)** – undesirable

The dial graphic was designed in Figma and UV-printed onto acrylic. Due to manufacturing constraints, the final markers represent 75-second increments rather than the intended 60 seconds, an acknowledged limitation retained due to time constraints.

<img width="167" height="193" alt="image" src="https://github.com/user-attachments/assets/c8c2aa1c-3398-4657-a28a-4c7e1474cbc9" />



### c. Build Challenges

During final assembly, several physical issues emerged:
- The acrylic dial height exceeded the available servo shaft length.
- A 3D-printed spacer reduced torque accuracy.
- The final demonstration used a paper prototype dial to ensure functional clarity.

These challenges highlighted the importance of earlier physical prototyping and mechanical testing, particularly when combining laser-cut and 3D-printed components.

---

## 4. Code & Device Logic

### a. Arduino / NodeMCU Basis

The embedded code was built upon example implementations from the Open Gauges Project, retaining core libraries for:
- Wi-Fi connectivity
- MQTT publish/subscribe.
- Servo motor control

(Hudson-Smith, 2025)

Additional libraries were introduced to support:
- LCD screen output
- JSON parsing via Arduino Json

<img width="255" height="279" alt="image" src="https://github.com/user-attachments/assets/eedf1b4b-f221-4f56-8b1d-593c77d3f5c8" />


### b. MQTT Topic Switching

A key extension beyond the example code was dynamic topic switching, allowing one device to subscribe to three different MQTT topics, one per dining location.

An array of topics is stored, and button presses trigger:
- Unsubscription from the current topic
- Index update
- Subscription to the selected topic

This enables direct selection rather than cycling, improving usability and responsiveness.

### c. JSON Parsing & Data Handling

Incoming MQTT messages are structured as JSON objects. The device extracts:

**queue time** → mapped to servo position.

Due to time limitations, LCD display of daily specials and prices was not fully implemented in the final build, though code scaffolding demonstrates how this would be achieved safely within LCD character limits.

Queue values are constrained to a maximum range to prevent the servo exceeding the physical limits of the dial.

<img width="373" height="175" alt="image" src="https://github.com/user-attachments/assets/3ce434f1-3524-4ba2-bb25-090bb78952be" />


---

## 5. Data Design & Live MQTT System

### a. Research-Informed Dataset Design

Rather than using arbitrary values, we grounded our dataset in on-site observations and staff conversations at:
- Pool Street Café
- Marshgate Café
- Marshgate Canteen

Staff were asked about:
- Busy periods
- Average service speed
- Queue behaviour
- Menu rotation and daily specials

These insights informed our assumptions and data structure.

<img width="486" height="249" alt="image" src="https://github.com/user-attachments/assets/089bb6e4-b34e-4bd6-8054-95c1e3f1572d" />


### b. Queue Time Model

Queue time was calculated using a simplified but transparent formula:

```
queue time = people_in_queue × service factor
```

Service factors differed by location based on observed service speed:
- **Pool Street Café**: ×1
- **Marshgate Café**: ×2
- **Marshgate Canteen**: ×3

This approach prioritised clarity and reproducibility over realism, appropriate for a demonstrative prototype.

(Eclipse Mosquitto, 2025)  
(Eclipse Foundation, 2024)

<img width="317" height="282" alt="image" src="https://github.com/user-attachments/assets/c4cf73a3-039b-42c1-8846-b63bcafc073b" />

### c. MQTT Architecture

Data is published using the following topic hierarchy:

<img width="409" height="218" alt="image" src="https://github.com/user-attachments/assets/72580a06-8a3c-4a0e-b481-45bf7980d1f9" />


```
student/CASA0019/gauge&grab/
   ├── pool_street
   ├── marshgate_cafe
   └── marshgate_canteen
```

Each message includes:
- Location
- People in queue
- Queue time
- Daily special
- Price

Data is generated every 2 seconds via a Python script using Dabo-style publishing, simulating a live operational system.

<img width="230" height="159" alt="image" src="https://github.com/user-attachments/assets/c1772c00-9a47-4af2-82c1-506d7d8d75a3" />



### d. Verification

The pipeline was validated using MQTT Explorer, confirming:
- Correct topic structure
- Live updates across all locations
- Data consistency between publisher, physical device, and digital twin

(Python Software Foundation, 2025)  
(Schiffler, 2025)

### e. Data Summary

```
Real Questions at Cafés
        ↓
Draft dataset (menus, multipliers, queue ranges)
        ↓
Python generator → Live values every 2 seconds
        ↓
MQTT Publish → student/CASA0019/gauge&grab/<location>
        ↓
Physical Gauge Reads (Arduino)
        ↓
Digital Twin Reads (Unity AR)
```

---

## 6. Digital Twin (Unity)

<img width="122" height="147" alt="image" src="https://github.com/user-attachments/assets/42ed12dc-2973-434c-9747-a7ad43d2e5dd" />


## a.	Purpose of the Digital Twin


The digital twin complements our physical gauge by using augmented reality to visualise our dataset into an interactive digital gauge and dashboards for each location. The AR gauge receives live data from MQTT enabling users to pick their chosen location and view queue times and specials efficiently without having to be in proximity to the physical gauge. To ensure the user would be able to view the gauge in AR, the gauge was turned into a prefab in unity to ensure better control of the 3D model. In the scene with the XR Origin game object selected the tap to place script is added as a component in the inspector window with the digital gauge prefab selected as the game object to instantiate (workshop 6 Cetools.org, 2026). It is vital that the AR object (the digital gauge prefab) only lives inside the tap to place component and not as a game object in the scene hierarchy. An AR object UI bridge script is added as a component to the AR object prefab to enable the button to allow users to transition from the prefab to the dashboard.

<img width="451" height="152" alt="image" src="https://github.com/user-attachments/assets/036c16a2-4702-4707-b52d-f92134a62c1c" />


The Dashboard expands on the gauge by turning historical and live data into digestible charts and tables that allow users to better plan and understand their canteen options (Workshop 3 Cetools.org, 2026). The dashboard exists as a child of the AR object. Each location's dashboard includes:
- Location name 
- A table that outlines the daily food specials for the week with a description of the food item, its price, caloric value, allergens, as well as an estimated serving time. This is incorporated in the dashboard in an image of each table. 
- A line graph that represents the locations weekly peak wait time. The graph enables users to plan and better understand wait time trends for the week. 
- Buttons were used to allow user to cycle from the AR object to the dashboard, A further three buttons were added to allow users to switch dashboards and return to the gauge. A dashboard navigation script ensures that buttons work smoothly and allow users switch between the dashboards.

<img width="261" height="165" alt="image" src="https://github.com/user-attachments/assets/e89f1986-b8aa-4dc2-8d18-c9dcbb986599" />


### b. Unity & MQTT Integration

Our digital twin system transforms data received from MQTT into interactive and dynamic representations through our digital gauge and dashboards. An MQTT manager game object with an MQTT manager script as a component is added to the scene. The MQTT manager receives messages from MQTT ensuring connection the MQTT broker and subscription to our topics. The transformation of MQTT data into our interactive representations is done through two MQTT controller scripts.

<img width="441" height="203" alt="image" src="https://github.com/user-attachments/assets/376914c2-0b94-4ba0-98bb-75dfd1d8c933" />


For the digital gauge, an MQTT controller script was attached as a component of our AR object. This script locates the MQTT manager in the scene and receives data from its subscribed topics (workshop 6 Cetools.org, 2026). The MQTT controller script ensures that the gauge only displays data from the user's chosen location and ignores messages related to the other locations. Further the script changes the messages which it receives as Json text into C# strings and integers. The queue time is then calculated in the script as well as ensuring that the queue time displayed works with our gauge's range of rotation, start and end values, and gauge needles rotation.

<img width="312" height="189" alt="image" src="https://github.com/user-attachments/assets/912e7b70-5cbb-4505-8b6f-8df967e56d3a" />


As the dashboard exists as a child of the AR object the MQTT WeeklyChart Controller dashboard controller script is placed as a component inside the dashboard. The dashboard controller script receives data from the same MQTT manager however this script composites the data received over time to allow for it to be analysed. The controller tracks the busiest times for each location and visualises via a line chart for each location that displays weekly peak times.

<img width="380" height="206" alt="image" src="https://github.com/user-attachments/assets/b900069a-0e24-4a0e-abc8-3a52c5f2db7a" />


### c. Limitations During Presentation

An issue faced was late integration of live data which was only successfully implemented the day of the presentation. This hindered our ability to assess whether the data was displayed and initialised correctly on the digital gauge and the line charts in the dashboard. This was further impacted as we faced issues with tap to place during the presentation that prevented the AR object from displaying as well as the button required to display the dashboard. 

We will ensure that issues like these are mitigated in the future by ensuring earlier end-to-end testing and ensuring that we document and film the tap to place function working prior to implementing the dashboard to ensure that a working demo was available for the presentation.

---

## 7. Individual Contributions

**Ethan Taylor**: Led physical device design, enclosure fabrication, dial graphics, servo integration, and Arduino logic.

**Madina Diallo**: Led project coordination, dataset design, MQTT architecture, Python data generation, system pipeline integration, documentation. 

**Yussr Osman Kamil Bashir**: Supported research, data structuring, testing, Unity dashboard development, and system validation.

---

## 8. Reflection

### a. What Worked Well
- Clear end-to-end data pipeline
- Shared MQTT feed driving physical + digital outputs.
- Intuitive, glanceable physical interface
- Strong alignment between research, data, and design

### b. Challenges and Limitations
- Simulated (dummy) data only.
- Simplified queue model
- Late physical and AR testing
- Mechanical constraints in final build

These were accepted to prioritise system architecture and real-time interaction.

---

## 9. Future Work

Future iterations could include:

- Address the 16-character LCD limitation by replacing it with a graphical OLED/TFT display or by using symbolic, colour-based indicators (e.g. LED bars or rings) to convey queue intensity. 

- A future improvement could also allow users to submit short, optional feedback digitally, and via an AR-based digital button after visiting a café, enabling us to compare lived experience with system estimates and continuously improve queue-time accuracy.

- Real queue detection via sensors or computer vision

- Integration with POS or menu management systems

- Predictive queue modelling using historical data.

- Personalisation (dietary needs, budget filters)

- Mobile companion app

- Staff-facing operational dashboard

Such developments would transform Gauge & Grab into a deployable smart-campus service (Letsa, 2017).

---

## 10. Conclusion

Gauge & Grab demonstrates how research-informed data, live MQTT messaging, and physical-digital visualisation can be combined into a coherent IoT system. 

We transformed simple café research, queue observations, menu requests, and staff Q&A into a structured dataset. We automated it with Python, broadcast it live through MQTT, and then connected both a physical gauge and an AR digital twin to the same data stream. This created a complete end-to-end IoT system built from real research, simulated data, and live messaging.

---

## References

- Aman, A. (2025) *Hand adjusting a smart thermostat on a white wall*. Vecteezy. Available at: https://www.vecteezy.com/photo/71521679-hand-adjusting-a-smart-thermostat-on-a-white-wall-increasing-the-room-temperature-for-comfort-and-energy-efficiency-the-digital-display-shows-19-degrees-celsius-modern-home-technology-concept

- Cetools.org (2026a) *03: Dashboard and Real Time Data*. Available at: https://workshops.cetools.org/codelabs/casa0019-03-unity-dashboard/index.html

- Cetools.org (2026b) *06: Unity AR Physical to Digital*. Available at: https://workshops.cetools.org/codelabs/casa0019-06-unity-ar-pd/index.html

- Eclipse Foundation (2024) *paho.mqtt.python: Eclipse Paho MQTT Python client library*. Available at: https://github.com/eclipse-paho/paho.mqtt.python

- Eclipse Mosquitto (2025) *MQTT: Lightweight messaging protocol*. Available at: https://mosquitto.org/

- Hudson-Smith, A. (2025) *WindSpeedGauge.ino*. Open-Gauges. Available at: https://github.com/ucl-casa-ce/Open-Gauges/blob/main/Arduino/WindSpeedGauge/WindSpeedGauge.ino

- Letsa, L. (2017) 'Assessing the Effect of Waiting Times on Restaurant Service Delivery in the Ho Municipality, Ghana', *European Business & Management*, 3(6), p. 113. https://doi.org/10.11648/j.ebm.20170306.13

- Low, E. et al. (2024) *CASA0019: Sensor Data Visualisation – SubRadar*. Available at: https://github.com/rorschachwilpeng/casa0019/

- Osborn, J.R. (2012) 'Helvetica and the New York City Subway System', *Design and Culture*, 4(1), pp. 120–122. https://doi.org/10.2752/175470812X13176523285796

- Python Software Foundation (2025) *Python documentation*. Available at: https://www.python.org/doc/

- Schiffler, A. (2025) *How to use the Paho MQTT client in Python*. Cedalo Blog. Available at: https://cedalo.com/blog/configuring-paho-mqtt-python-client-with-examples/

- UCL Centre for Advanced Spatial Analysis (2025) *DaBo: Data in a Box repository*. Available at: https://github.com/ucl-casa-ce/dabo

---

## Acknowledgement

This report would like to acknowledge that AI was used to assist with support for coding in our project. AI was used to assist with coding for our physical gauge, digital twin, and data. AI was used in line with UCL assessment guidelines and was not used to generate ideas or text.

---

## License

This project was developed as part of CASA0019: Sensor Data Visualisation at UCL Centre for Advanced Spatial Analysis.

---

*For questions or collaboration opportunities, please contact the project team through the UCL CASA Connected Environments programme.*
