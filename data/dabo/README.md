# Gauge & Grab — DaBo (Data Box)

This folder contains the **DaBo (Data Box)** system used in the *Gauge & Grab* project at **UCL East**.  
DaBo is responsible for transforming **research-informed dummy datasets** into **live MQTT data streams** that feed:

- a **physical queue gauge** (Arduino + LCD),
- a **digital gauge** (Unity AR),
- and **digital twin dashboards** (graphs and tables).

The system demonstrates a full data lifecycle — from **market research** to **live data delivery**.

---

## Project Context

Gauge & Grab helps users make informed food decisions by showing:
- estimated queue wait times,
- queue congestion bands (0–5, 5–10, 10+ minutes),
- and daily menu specials with prices,

across **three UCL East food locations**:
- Pool Street Café
- Marshgate Café
- Marshgate Canteen

---

## Dataset Overview

### What is the data?
The dataset represents:
- queue wait time (minutes),
- queue band (low / medium / high),
- daily menu special + price,
- timestamp and weekday,
- food location identifier.

### Where did the data come from?
The data was informed by:
- direct **observations** at all three locations,
- **staff Q&A** about peak times and queue behaviour,
- **daily menu requests** from each food outlet.

This information was used to design a **structured dummy dataset** that reflects realistic food-queue dynamics.

### Is this real data?
No — this is **dummy data**, but it is:
- research-informed,
- constrained,
- time-aware,
- and designed to behave realistically during demonstrations.

---

## From Dummy Data to Live MQTT Data

DaBo converts static dummy values into **live, changing data** using Python logic:

1. Dummy menu and queue parameters are defined per location.
2. Queue times are generated using **weighted randomness** to simulate peak and off-peak periods.
3. A **real-time multiplier** accelerates time so changes are visible during short demos.
4. Updated values are published to MQTT topics every few seconds.
5. Physical and digital devices subscribe to these topics and update instantly.

---

## MQTT Architecture

### Topic Structure
student/CASA0019/gauge&grab/{location}/state


Example:


student/CASA0019/gauge&grab/pool_street/state


### Payload Format
Each MQTT message is sent as JSON and includes:
```json
{
  "location": "pool_street",
  "queue_time": 7,
  "queue_band": "5-10",
  "daily_special": "Chicken Wrap",
  "price": 4.95,
  "weekday": "Tuesday",
  "timestamp": "2026-01-06T12:30:00"
}

---

## Devices Using This Data

The live MQTT data published by DaBo is consumed by three connected components, ensuring data consistency across physical and digital representations.

### Physical Gauge (Arduino)
The physical Gauge & Grab device subscribes to MQTT topics and displays:

- **Queue band** (0–5 mins, 5–10 mins, 10+ mins) using an analogue gauge pointer.
- **Daily menu special and price** on an LCD screen.

This allows users to quickly understand queue congestion and make informed decisions before joining a queue.

---

### Digital Gauge (Unity AR)
The digital gauge is implemented as an **Augmented Reality (AR) digital twin** in Unity.

- Unity subscribes to the **same MQTT topics** as the physical gauge.
- The AR gauge mirrors the queue band and menu data in real time.
- Location selection buttons trigger updates without duplicating data logic.

This ensures **data parity** between physical and digital devices.

---

### Digital Twin Dashboards
The digital twin dashboards provide expanded contextual information, including:

- **Peak queue trends** visualised as line graphs.
- **Daily specials tables** showing menu items and prices.
- **Location-specific analytics** for each food outlet.

Dashboards update live as new MQTT messages are published.

---

## Running DaBo

### Requirements
- Python 3.9 or higher
- MQTT broker (e.g. Mosquitto)
- Python virtual environment (recommended)

---

### Run Instructions

From the DaBo directory:

```bash
python main.py

## Running DaBo

Once running, **DaBo continuously generates and publishes live MQTT data updates** for all three food locations.  
These updates are streamed in real time and consumed simultaneously by the **physical queue gauge**, **digital gauge**, and **digital twin dashboards**, ensuring data consistency across all system components.

---

## Limitations

This project uses **simulated (dummy) data** to demonstrate a complete end-to-end data pipeline rather than a fully sensor-driven deployment.

The following limitations are acknowledged:

- Queue times are simulated and are **not derived from real-time sensing hardware**.
- Human behaviour variability (for example, sudden rushes or staffing changes) is **simplified** within the data model.
- Data accuracy depends on **assumptions informed by on-site observations and staff interviews**, rather than continuous automated measurement.

These limitations are accepted as part of a **prototyping and demonstrative approach**, focused on exploring system architecture, live data flow, and physical–digital integration rather than producing exact real-world predictions.

---

## Future Development

Future iterations of the project could significantly enhance **realism, accuracy, and scalability**. Potential improvements include:

- Embedding **live user Q&A feedback forms** at food locations to collect real user-reported waiting times and experiences.
- Comparing **user-reported wait times** with simulated values to improve calibration and reliability.
- Introducing **machine learning models** to predict queue length and waiting time based on historical patterns.
- Integrating **real-time sensing technologies**, such as infrared people counters or camera-based detection systems.
- Storing **historical MQTT data** to enable long-term trend analysis, performance optimisation, and operational insights.

Together, these developments would support the transition from a **conceptual prototype** toward a **fully data-driven smart queue monitoring system**.

---

