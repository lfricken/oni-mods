# Created by lfricken https://github.com/lfricken/oni-mods
# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
import pylab
import copy
from collections import deque


class Rocket:
	fuel_efficiency: float
	oxidizer_efficiency: float
	engine_mass: int
	cargo_bays: int
	other_modules: int
	additional_mass: float

	def __init__(self, fuel_efficiency, oxidizer_efficiency, engine_mass, cargo_bays=0, other_modules=0, additional_mass=0):
		self.fuel_efficiency = fuel_efficiency
		self.oxidizer_efficiency = oxidizer_efficiency
		self.engine_mass = engine_mass
		self.cargo_bays = cargo_bays
		self.other_modules = other_modules
		self.additional_mass = additional_mass


def make_rockets(num: int) -> [Rocket]:
	rocket_copies: [Rocket] = []
	for i in range(num):
		rocket_copies.append(Rocket(60, 1.33, 500, 5))
	return rocket_copies


# your furthest rocket should go first!
rockets: [Rocket] = make_rockets(2)

rockets[0].engine_mass = 100
rockets[0].fuel_efficiency = 180

stop_graph_at_peak_range: bool = False
fuel_tank_capacity = 900
oxy_tank_capacity = 2700
empty_tank_mass = 100

capsule_mass = 200
cargo_bay_mass = 2000
other_module_mass = 200


def get_num_tanks(amount: float, amount_per: float) -> int:
	return math.ceil(max(float(1), amount) / amount_per)


def get_num_fuel_tanks(kg_fuel: float) -> int:
	return get_num_tanks(kg_fuel, fuel_tank_capacity)


def get_num_oxy_tanks(kg_oxy: float) -> int:
	return get_num_tanks(kg_oxy, oxy_tank_capacity)


def get_weight(kg_fuel: float, kg_oxy: float, rocket: Rocket) -> float:
	num_fuel_tanks = get_num_fuel_tanks(kg_fuel)
	num_oxy_tanks = get_num_oxy_tanks(kg_oxy)

	fuel = num_fuel_tanks * empty_tank_mass + kg_fuel
	oxidizer = num_oxy_tanks * empty_tank_mass + kg_oxy
	capsule_engine = capsule_mass + rocket.engine_mass
	storage = rocket.cargo_bays * cargo_bay_mass
	other = rocket.other_modules * other_module_mass
	return fuel + oxidizer + capsule_engine + storage + other


def distance_penalty(weight: float) -> float:
	weight: float = float(weight)
	return max(weight, (weight / 300.0) ** 3.2)


def distance(kg_fuel: float, kg_oxy: float, km_per_kg_fuel: float, oxy_efficiency: float) -> float:
	return min(kg_fuel, kg_oxy) * km_per_kg_fuel * oxy_efficiency


def calc_total_distance(kg_fuel: float, rocket: Rocket):
	kg_oxy = kg_fuel

	weight = get_weight(kg_fuel, kg_oxy, rocket)
	penalty = distance_penalty(weight)

	max_dist = distance(kg_fuel, kg_oxy, rocket.fuel_efficiency, rocket.oxidizer_efficiency)
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

	is_getting_worse = 0 > a
	if stop_graph_at_peak_range:
		is_getting_worse: bool = True

	return is_getting_worse and a > b > c


def build_legend(rocket: Rocket) -> str:
	return '[' + str(rocket.fuel_efficiency) + ',' + str(rocket.engine_mass) + ',' + str(rocket.cargo_bays) + ']'


def main():
	max_fuel_amount = 50000
	max_distance = 0
	distance_delta_queue: deque = deque()

	for rocket in rockets:
		x_axis: [int] = []
		y_axis: [int] = []
		for fuel_amount in range(max_fuel_amount):
			new_range = calc_total_distance(fuel_amount, rocket)

			distance_delta_queue.append(new_range)
			if len(distance_delta_queue) >= 3:
				if max_distance == 0 and only_gets_worse(distance_delta_queue):
					max_distance = new_range
					max_fuel_amount = fuel_amount
					break  # rocket distance will never go up again

				distance_delta_queue.popleft()

			x_axis.append(fuel_amount)
			y_axis.append(max(float(0), new_range))

		if len(x_axis) == 0:
			print('Your rocket was so bad that it cant even get 1 km of range! Try improving fuel efficiency or reducing weight.')

		plt.plot(x_axis, y_axis, label=build_legend(rocket))

	plt.xlabel('Fuel in Kg')
	plt.ylabel('Range in Km')

	pylab.legend(title='[Efficiency,EngineMass,CargoBays]', loc='upper left')

	plt.show()


test_this()  # uncomment to test
main()
