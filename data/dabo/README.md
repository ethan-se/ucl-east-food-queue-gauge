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
