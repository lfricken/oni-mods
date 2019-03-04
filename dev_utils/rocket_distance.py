# Created by lfricken https://github.com/lfricken/oni-mods
# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
import pylab
from collections import deque

# what is on your rocket?
cargo_bays = 0
other_modules = 0

# must be in descending efficiency! km of range per kg of fuel burned
fuel_efficiencies = [60, 40, 20]
fuel_names = ['Hydrogen', 'Petroleum', 'Steam']

# oxylite is 1.0, lox is 1.33
oxidizer_efficiency = 1.33

stop_graph_at_peak_range: bool = True
fuel_tank_capacity = 900
oxy_tank_capacity = 2700
empty_tank_mass = 100

capsule_mass = 200
engine_mass = 500

cargo_bay_mass = 2000
other_module_mass = 200


def get_num_tanks(amount: float, amount_per: float) -> int:
	return math.ceil(max(float(1), amount) / amount_per)


def get_num_fuel_tanks(kg_fuel: float) -> int:
	return get_num_tanks(kg_fuel, fuel_tank_capacity)


def get_num_oxy_tanks(kg_oxy: float) -> int:
	return get_num_tanks(kg_oxy, oxy_tank_capacity)


def get_weight(kg_fuel: float, kg_oxy: float, cargo_bays_units: int, other_modules_units: int) -> float:
	num_fuel_tanks = get_num_fuel_tanks(kg_fuel)
	num_oxy_tanks = get_num_oxy_tanks(kg_oxy)

	fuel = num_fuel_tanks * empty_tank_mass + kg_fuel
	oxidizer = num_oxy_tanks * empty_tank_mass + kg_oxy
	capsule_engine = capsule_mass + engine_mass
	storage = cargo_bays_units * cargo_bay_mass
	other = other_modules_units * other_module_mass
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
	assert (1 == get_num_tanks(1, fuel_tank_capacity))
	assert (1 == get_num_tanks(2, fuel_tank_capacity))
	assert (1 == get_num_tanks(fuel_tank_capacity, fuel_tank_capacity))
	assert (2 == get_num_tanks(fuel_tank_capacity + 1, fuel_tank_capacity))
	assert (2 == get_num_tanks(fuel_tank_capacity * 2, fuel_tank_capacity))
	assert (3 == get_num_tanks(fuel_tank_capacity * 2 + 1, fuel_tank_capacity))


def only_gets_worse(distance_delta_queue: deque) -> bool:
	a = distance_delta_queue[0]
	b = distance_delta_queue[1]
	c = distance_delta_queue[2]
	d = distance_delta_queue[3]

	is_getting_worse = 0 > a
	if stop_graph_at_peak_range:
		is_getting_worse: bool = True

	return is_getting_worse and a > b > c > d


def main():
	max_fuel_amount = 50000
	max_distance = 0
	distance_delta_queue: deque = deque()

	for i in range(len(fuel_efficiencies)):
		efficiency = fuel_efficiencies[i]
		is_most_efficient_fuel: bool = efficiency == fuel_efficiencies[0]
		x_axis: [int] = []
		y_axis: [int] = []
		for fuel_amount in range(max_fuel_amount):
			new_range = calc_total_distance(fuel_amount, efficiency)

			if fuel_amount > 900 and efficiency == 20:
				break

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

		plt.plot(x_axis, y_axis, label=fuel_names[i])

	plt.xlabel('Fuel in Kg')
	plt.ylabel('Range in Km')

	pylab.legend(title='Fuel Efficiency', loc='upper left')

	plt.text(max_fuel_amount * 0.25, max_distance * 1.12, 'Cargo Bays: ' + str(cargo_bays), ha='center', va='center')
	plt.text(max_fuel_amount * 0.25, max_distance * 1.18, 'Other Modules: ' + str(other_modules), ha='center', va='center')
	plt.text(max_fuel_amount * 0.75, max_distance * 1.12, 'Engine Mass: ' + str(engine_mass) + 'kg', ha='center', va='center')
	plt.text(max_fuel_amount * 0.75, max_distance * 1.18, 'Oxidizer Boost: ' + str(oxidizer_efficiency) + 'x', ha='center', va='center')

	plt.show()


test_this()  # uncomment to test
main()
