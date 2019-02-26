# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
from collections import deque

num_storage = 1
num_other = 0

km_per_kg_fuel = 60
oxidizer_efficiency = 1.33

kg_fuel_per_tank = 900
kg_oxy_per_tank = 2700
kg_per_dry_tank = 100

kg_per_capsule = 200
kg_per_engine = 500

kg_per_storage_container = 2000
kg_per_other = 200


def get_num_tanks(amount: float, amount_per: float) -> int:
	return math.ceil(amount / amount_per)


def get_num_fuel_tanks(kg_fuel: float) -> int:
	return get_num_tanks(kg_fuel, kg_fuel_per_tank)


def get_num_oxy_tanks(kg_oxy: float) -> int:
	return get_num_tanks(kg_oxy, kg_oxy_per_tank)


def get_weight(kg_fuel: float, kg_oxy: float, num_storage_units: int, num_other_units: int) -> float:
	num_fuel_tanks = get_num_fuel_tanks(kg_fuel)
	num_oxy_tanks = get_num_oxy_tanks(kg_oxy)

	fuel = num_fuel_tanks * kg_per_dry_tank + kg_fuel
	oxidizer = num_oxy_tanks * kg_per_dry_tank + kg_oxy
	capsule_engine = kg_per_capsule + kg_per_engine
	storage = num_storage_units * kg_per_storage_container
	other = num_other_units * kg_per_other
	return fuel + oxidizer + capsule_engine + storage + other


def distance_penalty(weight: float) -> float:
	weight: float = float(weight)
	return max(weight, (weight / 300.0) ** 3.2)


def distance(kg_fuel: float, kg_oxy: float) -> float:
	return oxidizer_efficiency * min(kg_fuel, kg_oxy) * km_per_kg_fuel


def calc_total_distance(kg_fuel: float):
	kg_oxy = kg_fuel

	weight = get_weight(kg_fuel, kg_oxy, num_storage, num_other)
	penalty = distance_penalty(weight)

	max_dist = distance(kg_fuel, kg_oxy)
	final_distance = max_dist - penalty
	return final_distance


def test_this():
	assert (0 == get_num_tanks(0, kg_fuel_per_tank))
	assert (1 == get_num_tanks(1, kg_fuel_per_tank))
	assert (1 == get_num_tanks(2, kg_fuel_per_tank))
	assert (1 == get_num_tanks(kg_fuel_per_tank, kg_fuel_per_tank))
	assert (2 == get_num_tanks(kg_fuel_per_tank + 1, kg_fuel_per_tank))
	assert (2 == get_num_tanks(kg_fuel_per_tank * 2, kg_fuel_per_tank))
	assert (3 == get_num_tanks(kg_fuel_per_tank * 2 + 1, kg_fuel_per_tank))


def main():
	max_fuel_amount = 10000
	distance_delta_queue: deque = deque()

	x_axis: [int] = []
	y_axis: [int] = []
	for fuel_amount in range(max_fuel_amount):
		new_range = calc_total_distance(fuel_amount)
		distance_delta_queue.append(new_range)

		if len(distance_delta_queue) > 3:
			distance_delta_queue.pop()

		# rocket distance will never go up again
		if distance_delta_queue[0] > new_range and distance_delta_queue[1] > new_range and distance_delta_queue[2] > new_range and new_range < 0:
			break
		else:
			x_axis.append(fuel_amount)
			y_axis.append(new_range)

	if len(x_axis) == 0:
		print("Your rocket was so bad that it can't even get 1 km of range! Try improving fuel efficiency or reducing weight.")

	plt.plot(x_axis, y_axis)
	plt.xlabel("Fuel in Kg")
	plt.ylabel("Distance in Km")
	plt.show()


test_this()  # uncomment to test
main()
