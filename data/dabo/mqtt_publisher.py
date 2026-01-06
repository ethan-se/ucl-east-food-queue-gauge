import json
import paho.mqtt.client as mqtt

BROKER = "localhost"
PORT = 1883
ROOT_TOPIC = "student/CASA0019/gauge&grab"

client = mqtt.Client()
client.connect(BROKER, PORT, 60)

def publish_data(location, payload):
    topic = f"{ROOT_TOPIC}/{location}/state"
    client.publish(topic, json.dumps(payload))
