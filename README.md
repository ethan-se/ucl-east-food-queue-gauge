
![Picture of Gauge and Go physical device](./docs/physicalGauge-cropped.jpg)

# Gauge & Grab{link}(https://youtube.com/shorts/w2yIuAzpJI0?si=rPMWGunTOieLFzZq)

**A Physical-Digital IoT System for Campus Dining Decisions**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Overview

Gauge & Grab is an IoT-based decision support system designed to help UCL East campus users quickly choose where to eat by visualizing real-time queue data through both a physical gauge and an augmented reality digital twin.

The system addresses a common campus challenge: uncertainty about where to dine during peak hours. By combining live MQTT data streams with intuitive physical and digital interfaces, users can make informed dining decisions in under 5 seconds.

**Research Question:**  
*How can food-service data be presented in a fast, simple, and meaningful way to support informed decision-making on campus?*

---

## Features

### Physical Gauge Device
- **Instant Queue Visualization**: Servo-driven dial with color-coded zones (green/yellow/red)
- **Multi-Location Support**: Three-button interface for Pool Street Café, Marshgate Café, and Marshgate Canteen
- **Campus-Integrated Design**: Minimalist black acrylic enclosure with white Helvetica typography
- **Live MQTT Integration**: Real-time data updates via Wi-Fi connectivity

### Digital Twin (Unity AR)
- **Augmented Reality Gauge**: Mobile AR visualization of queue times
- **Interactive Dashboards**: Location-specific data including:
  - Weekly menu specials with prices, calories, and allergens
  - Historical peak-time analysis via line charts
  - Service time estimates
- **Seamless Navigation**: Button-driven interface for switching between locations and views

### Data Pipeline
- **Research-Informed Dataset**: Based on on-site observations and staff interviews
- **Live MQTT Architecture**: Pub/sub system with structured topic hierarchy
- **Transparent Queue Model**: `queue_time = people_in_queue × service_factor`
- **Python Data Generator**: Simulates live operational data every 2 seconds

---

## System Architecture

```
Real-World Research (Café Observations & Staff Interviews)
                ↓
      Dataset Design (Menus, Queue Models)
                ↓
    Python Generator → Live MQTT Stream
                ↓
       MQTT Broker (Mosquitto)
        ↙            ↘
Physical Gauge      Digital Twin
 (Arduino)          (Unity AR)
```

**MQTT Topic Structure:**
```
student/CASA0019/gauge&grab/
   ├── pool_street
   ├── marshgate_cafe
   └── marshgate_canteen
```

Each message includes: location, queue length, wait time, daily special, and price.

---

## Hardware Components

- **Microcontroller**: NodeMCU ESP8266
- **Display**: 16-character LCD screen
- **Actuator**: Servo motor with custom dial graphic
- **Interface**: 3× tactile buttons + LED indicators
- **Enclosure**: Laser-cut black acrylic with UV-printed graphics
- **Mounting**: Magnetic back plate

---

## Software Stack

### Embedded (Arduino)
- **Core Libraries**: Wi-Fi, MQTT (PubSubClient), Servo control
- **Extensions**: ArduinoJson for data parsing, LiquidCrystal for LCD
- **Key Features**:
  - Dynamic MQTT topic switching
  - JSON message parsing
  - Servo position mapping with physical constraints

### Digital Twin (Unity)
- **Platform**: Unity with AR Foundation
- **MQTT Integration**: Custom MQTT Manager for broker connection
- **Controllers**:
  - `MQTTController`: Real-time gauge updates
  - `MQTTWeeklyChartController`: Historical data visualization
- **AR Features**: Tap-to-place functionality with prefab instantiation

### Data Generation (Python)
- **Libraries**: `paho-mqtt` for publishing
- **Publishing Rate**: Every 2 seconds
- **Data Format**: Structured JSON with validated ranges

---

## Installation & Setup

### Physical Device

1. **Hardware Assembly**:
   - Connect servo to GPIO pin
   - Wire LCD display and buttons
   - Mount components in laser-cut enclosure

2. **Arduino Configuration**:
   ```cpp
   // Update Wi-Fi credentials
   const char* ssid = "YOUR_SSID";
   const char* password = "YOUR_PASSWORD";
   
   // Configure MQTT broker
   const char* mqtt_server = "mqtt.cetools.org";
   ```

3. **Upload Code**:
   - Install required libraries via Arduino Library Manager
   - Compile and upload to NodeMCU

### Digital Twin

<img width="892" height="564" alt="Screenshot 2026-01-09 at 18 26 07" src="https://github.com/user-attachments/assets/ec39c62e-2964-46b6-962c-7c037fd153ca" />



1. **Unity Setup**:
   - Open project in Unity 2021.3 or later
   - Install AR Foundation and platform-specific packages
   - Configure MQTT broker address in MQTT Manager

2. **Build for Mobile**:
   - iOS: Xcode 14+
   - Android: Android Studio with AR Core support

### Data Pipeline

1. **Install Python Dependencies**:
   ```bash
   pip install paho-mqtt
   ```

2. **Run Data Generator**:
   ```bash
   python data_generator.py
   ```

3. **Verify with MQTT Explorer**:
   - Connect to `mqtt.cetools.org`
   - Subscribe to `student/CASA0019/gauge&grab/#`

---

## Usage

### Physical Gauge
1. Press a location button (Pool Street, Marshgate Café, or Marshgate Canteen)
2. LED indicator confirms selection
3. Dial updates to show current queue time
4. Color zone indicates wait acceptability (green < 5 min, yellow 5-10 min, red > 10 min)

### Digital Twin
1. Launch AR app and point camera at a flat surface
2. Tap to place digital gauge
3. Select location via on-screen buttons
4. View real-time queue data
5. Tap dashboard button to see weekly specials and peak-time charts
6. Navigate between location dashboards using navigation buttons

---

## Research Foundation

### Data Collection Methodology
- **On-site observations** at three UCL East dining locations
- **Staff interviews** about busy periods, service speed, and menu rotation
- **Queue behavior analysis** during peak hours (12:00-14:00)

### Service Factor Calibration
Based on observed service characteristics:
- **Pool Street Café**: ×1 (fastest service)
- **Marshgate Café**: ×2 (moderate service)
- **Marshgate Canteen**: ×3 (slower, seated service)

---

## Known Limitations

- **Simulated Data**: Current system uses generated data rather than live sensors
- **Dial Calibration**: Physical markers represent 75-second increments (manufacturing constraint)
- **LCD Character Limit**: 16-character display restricts menu detail
- **Mechanical Challenges**: Servo torque affected by 3D-printed spacer in final build
- **AR Stability**: Tap-to-place functionality requires optimal lighting conditions

---

## Future Enhancements

### Short-term
- [ ] Replace LCD with graphical OLED/TFT display
- [ ] Implement user feedback mechanism via AR interface
- [ ] Improve mechanical assembly for better servo accuracy

### Long-term
- [ ] Real queue detection via computer vision or presence sensors
- [ ] Integration with POS systems for live menu data
- [ ] Predictive modeling using historical trends
- [ ] Personalization filters (dietary needs, budget)
- [ ] Mobile companion app
- [ ] Staff-facing operational dashboard

---

## Contributors

- **Ethan Taylor**: Physical device design, enclosure fabrication, Arduino development
- **Madina Diallo**: Project coordination, dataset design, MQTT architecture, documentation
- **Yussr Osman Kamil Bashir**: Research support, Unity development, system validation

---

## References

- Letsa, L. (2017) 'Assessing the Effect of Waiting Times on Restaurant Service Delivery', *European Business & Management*, 3(6), p. 113
- Low, E. et al. (2024) *SubRadar: CASA0019 Sensor Data Visualisation*
- Hudson-Smith, A. (2025) *Open-Gauges Project*
- Eclipse Mosquitto (2025) *MQTT Protocol Documentation*

**Workshop Resources:**
- [CASA0019 Unity Dashboard Tutorial](https://workshops.cetools.org/codelabs/casa0019-03-unity-dashboard/)
- [CASA0019 Unity AR Physical to Digital](https://workshops.cetools.org/codelabs/casa0019-06-unity-ar-pd/)

---

## License

This project was developed as part of CASA0019: Sensor Data Visualisation at UCL Centre for Advanced Spatial Analysis.

---

## Acknowledgments

Special thanks to UCL East dining staff for their insights, and to the Connected Environments teaching team for technical guidance throughout the project.
