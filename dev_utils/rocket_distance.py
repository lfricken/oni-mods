# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
import pylab
from collections import deque

# what is on your rocket?
cargo_bays = 2
other_modules = 0

# must be in descending efficiency!
km_per_kg_fuel_array = [110, 80, 60, 40]
oxidizer_efficiency = 1.33

cut_graph_at_peak: bool = True
kg_fuel_per_tank = 900
kg_oxy_per_tank = 2700
kg_per_dry_tank = 100

kg_per_capsule = 200
kg_per_engine = 500

kg_per_storage_container = 2000
kg_per_other = 200


def get_num_tanks(amount: float, amount_per: float) -> int:
	return math.ceil(max(float(1), amount) / amount_per)


def get_num_fuel_tanks(kg_fuel: float) -> int:
	return get_num_tanks(kg_fuel, kg_fuel_per_tank)


def get_num_oxy_tanks(kg_oxy: float) -> int:
	return get_num_tanks(kg_oxy, kg_oxy_per_tank)


def get_weight(kg_fuel: float, kg_oxy: float, cargo_bays_units: int, other_modules_units: int) -> float:
	num_fuel_tanks = get_num_fuel_tanks(kg_fuel)
	num_oxy_tanks = get_num_oxy_tanks(kg_oxy)

	fuel = num_fuel_tanks * kg_per_dry_tank + kg_fuel
	oxidizer = num_oxy_tanks * kg_per_dry_tank + kg_oxy
	capsule_engine = kg_per_capsule + kg_per_engine
	storage = cargo_bays_units * kg_per_storage_container
	other = other_modules_units * kg_per_other
	return fuel + oxidizer + capsule_engine + storage + other


def distance_penalty(weight: float) -> float:
	weight: float = float(weight)
	return max(weight, (weight / 300.0) ** 3.2)


def distance(kg_fuel: float, kg_oxy: float, km_per_kg_fuel: float) -> float:
	return oxidizer_efficiency * min(kg_fuel, kg_oxy) * km_per_kg_fuel


def calc_total_distance(kg_fuel: float, km_per_kg_fuel: float):
	kg_oxy = kg_fuel

	weight = get_weight(kg_fuel, kg_oxy, cargo_bays, other_modules)
	penalty = distance_penalty(weight)

	max_dist = distance(kg_fuel, kg_oxy, km_per_kg_fuel)
	final_distance = max_dist - penalty
	return final_distance


def test_this():
	assert (1 == get_num_tanks(1, kg_fuel_per_tank))
	assert (1 == get_num_tanks(2, kg_fuel_per_tank))
	assert (1 == get_num_tanks(kg_fuel_per_tank, kg_fuel_per_tank))
	assert (2 == get_num_tanks(kg_fuel_per_tank + 1, kg_fuel_per_tank))
	assert (2 == get_num_tanks(kg_fuel_per_tank * 2, kg_fuel_per_tank))
	assert (3 == get_num_tanks(kg_fuel_per_tank * 2 + 1, kg_fuel_per_tank))


def only_gets_worse(distance_delta_queue: deque) -> bool:
	a = distance_delta_queue[0]
	b = distance_delta_queue[1]
	c = distance_delta_queue[2]
	d = distance_delta_queue[3]

	is_getting_worse = 0 > a
	if cut_graph_at_peak:
		is_getting_worse: bool = True

	return is_getting_worse and a > b > c > d


def main():
	max_fuel_amount = 50000
	max_distance = 0
	distance_delta_queue: deque = deque()

	for efficiency in km_per_kg_fuel_array:
		is_most_efficient_fuel: bool = efficiency == km_per_kg_fuel_array[0]
		x_axis: [int] = []
		y_axis: [int] = []
		for fuel_amount in range(max_fuel_amount):
			new_range = calc_total_distance(fuel_amount, efficiency)

			distance_delta_queue.append(new_range)
			if len(distance_delta_queue) >= 4:
				if is_most_efficient_fuel and only_gets_worse(distance_delta_queue):
					max_distance = new_range
					max_fuel_amount = fuel_amount
					break  # rocket distance will never go up again

				distance_delta_queue.popleft()

			x_axis.append(fuel_amount)
			y_axis.append(max(float(0), new_range))

		if len(x_axis) == 0:
			print('Your rocket was so bad that it cant even get 1 km of range! Try improving fuel efficiency or reducing weight.')

		plt.plot(x_axis, y_axis, label=str(efficiency) + 'km per kg')

	plt.xlabel('Fuel in Kg')
	plt.ylabel('Distance in Km')
	pylab.legend(loc='upper left')
	plt.text(max_fuel_amount * 0.3, max_distance * 1.12, 'Cargo Bays: ' + str(cargo_bays), ha='center', va='center')
	plt.text(max_fuel_amount * 0.6, max_distance * 1.12, 'Other: ' + str(other_modules), ha='center', va='center')
	plt.show()


test_this()  # uncomment to test
main()
