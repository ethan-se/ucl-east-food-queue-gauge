1. main.py — Live Data Orchestrator

Purpose:
The core script that runs the live data simulation and publishing loop.

Responsibilities:

Loads the device definitions from device_template.json

Generates simulated queue data at a fixed interval

Calculates queue time using service-speed multipliers

Adds contextual metadata (location, weekday, timestamp)

Publishes JSON payloads to MQTT topics

Key Logic Implemented:

queue_time = number_of_people × service_factor


Why it matters:
This file transforms static dummy values into live, time-based data, making the system behave like a real operational environment.

"""
DaBo main script customised for:
student/CASA0019/gauge&grab

It:
- Loads device definitions from device_template.json
- Simulates time using FREQUENCY and REALTIME_MULTIPLIER from .env
- Generates synthetic sensor values using data_generator.generate_data
- Adds queue_time_min, queue_band, weekday and location
- Publishes JSON to MQTT using MQTTPublisher

Expected final MQTT topics (one per location):
  student/CASA0019/gauge&grab/pool_street
  student/CASA0019/gauge&grab/marshgate_cafe
  student/CASA0019/gauge&grab/marshgate_canteen
"""

import json
import os
import time
from datetime import datetime

from data_generator import generate_data
from mqtt_publisher import MQTTPublisher


# ----- CONFIG HELPERS --------------------------------------------------------


def get_frequency() -> float:
    """
    Read FREQUENCY (seconds between publishes) from environment.
    Falls back to 1 second if not set or invalid.
    """
    try:
        return float(os.getenv("FREQUENCY", "1"))
    except ValueError:
        return 1.0


def get_realtime_multiplier() -> float:
    """
    Read REALTIME_MULTIPLIER from environment.
    Controls how quickly the simulated 24h clock runs.

      1   => real time      (1 sec real = 1 sec simulated)
      60  => 1 second = 1 simulated minute
      3600=> 1 second = 1 simulated hour
    """
    try:
        return float(os.getenv("REALTIME_MULTIPLIER", "60"))
    except ValueError:
        return 60.0


# ----- QUEUE LOGIC -----------------------------------------------------------

# Minutes per person per location
LOCATION_FACTORS = {
    "pool_street": 1,          # 1 min per person
    "marshgate_cafe": 2,       # 2 mins per person
    "marshgate_canteen": 3,    # 3 mins per person
}


def compute_queue_band(queue_time_min: int) -> int:
    """
    Convert queue time (minutes) into pointer band:
      0 => 0–5 mins
      1 => 6–10 mins
      2 => 10+ mins
    """
    if queue_time_min <= 5:
        return 0
    elif queue_time_min <= 10:
        return 1
    else:
        return 2


# ----- DEVICE LOADING --------------------------------------------------------


class Device:
    """
    Simple holder for each virtual device/location.
    device_type: e.g. "pool_street"
    device_id:   same as device_type so topics end with the location name
    sensors:     sensor configuration dict from device_template.json
    """

    def __init__(self, device_type: str, sensors: dict):
        self.device_type = device_type
        self.device_id = device_type  # ensures topic suffix is just "pool_street"
        self.sensors = sensors


def load_devices_from_template(template_path: str = "device_template.json"):
    """
    Read device_template.json and build a list of Device objects.

    Expected structure per device entry:
      {
        "device_type": "pool_street",
        "count": 1,
        "root_topic": "",
        "sensors": { ... }
      }
    We ignore count and root_topic here and create ONE device per entry.
    """
    with open(template_path, "r", encoding="utf-8") as f:
        template = json.load(f)

    devices = []
    for entry in template:
        device_type = entry["device_type"]
        sensors = entry["sensors"]
        devices.append(Device(device_type=device_type, sensors=sensors))
    return devices


# ----- MAIN LOOP -------------------------------------------------------------


def main():
    # Load devices and MQTT publisher
    devices = load_devices_from_template("device_template.json")
    publisher = MQTTPublisher()

    frequency = get_frequency()
    rt_mult = get_realtime_multiplier()

    print("DaBo Gauge&Grab publisher starting...")
    print(f"  FREQUENCY          = {frequency} s")
    print(f"  REALTIME_MULTIPLIER= {rt_mult}")
    print(f"  Devices: {[d.device_id for d in devices]}")

    # Simulated time in seconds since start of "day"
    simulated_seconds = 0.0

    while True:
        # Simulated time for this tick
        # Wrap at 24 hours to keep within a single day
        sim_sec_day = simulated_seconds % (24 * 3600)
        hour = int(sim_sec_day // 3600)
        minute = int((sim_sec_day % 3600) // 60)
        second = int(sim_sec_day % 60)

        # Real weekday (0=Mon..6=Sun)
        weekday = datetime.now().weekday()

        for device in devices:
            # 1) Generate base sensor values from template
            base_data = generate_data(device.sensors, hour, minute, second)

            # 2) Add common metadata
            data = dict(base_data)  # copy so we can extend safely
            data["location"] = device.device_type
            data["weekday"] = weekday

            # 3) Queue calculations
            people = int(data.get("people_count", 0))

            # Treat weekends as closed: force queue to 0 people
            if weekday >= 5:  # 5=Saturday, 6=Sunday
                people = 0

            factor = LOCATION_FACTORS.get(device.device_type, 1)
            queue_time = people * factor  # integer minutes

            data["people_count"] = people
            data["queue_time_min"] = int(queue_time)
            data["queue_band"] = compute_queue_band(data["queue_time_min"])

            # 4) Publish to MQTT (topic = MQTT_TOPIC/device_id)
            publisher.publish(device_id=device.device_id, data=data)

        # 5) Wait FREQUENCY seconds and advance simulated time
        time.sleep(frequency)
        simulated_seconds += frequency * rt_mult


if __name__ == "__main__":
    main()
