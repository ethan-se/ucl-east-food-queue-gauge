QUEUE_BANDS = {
    (0, 5): "0-5",
    (6, 10): "5-10",
    (11, 60): "10+"
}

MENUS = {
    "pool_street": [("Veggie Wrap", 4.50), ("Chicken Wrap", 4.95)],
    "marshgate_cafe": [("Pasta Box", 5.50), ("Panini", 4.80)],
    "marshgate_canteen": [("Curry Bowl", 6.20), ("Rice & Veg", 5.90)]
}

def get_queue_band(minutes):
    for (low, high), band in QUEUE_BANDS.items():
        if low <= minutes <= high:
            return band
    return "10+"

def generate_data(location, multiplier):
    now = datetime.now()
    weekday = now.strftime("%A")

    base_queue = random.randint(1, 15)
    queue_time = int(base_queue * (multiplier / 60))

    special, price = random.choice(MENUS[location])

    return {
        "location": location,
        "queue_time": queue_time,
        "queue_band": get_queue_band(queue_time),
        "daily_special": special,
        "price": price,
        "weekday": weekday,
        "timestamp": now.isoformat()
    }
