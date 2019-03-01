# Created by lfricken https://github.com/lfricken/oni-mods
# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
import pylab
import copy
from collections import deque

stop_graph_at_peak_range: bool = True

# magic numbers
parabolic_extraction = 3.333

#
r1_base_efficiency = 2
r2_base_efficiency = 4
r3_base_efficiency = 6.5
r4_base_efficiency = 9


real_hydrolox = 1
oni_hydrolox = 1

# range penalties
cargo_penalty = 35000
science_penalty = 12000
sight_penalty = 4000
booster_boost = 17000

# range equation
miles_per_int = 40000.0
exponent = 2.0
fuel_scalar = 900.0  # fuel per int
efficiency_scalar = oni_hydrolox / real_hydrolox
range_scalar = parabolic_extraction * miles_per_int





class Rocket:
	def __init__(self, fuel_efficiency, oxidizer_efficiency, engine_penalty, boosters=0, cargo_bays=0, science_bays=0, vision_bays=0):
		self.fuel_efficiency = fuel_efficiency
		self.oxidizer_efficiency = oxidizer_efficiency
		self.engine_penalty = engine_penalty
		self.max_fuel = 999999

		self.boosters = boosters

		self.cargo_bays = cargo_bays
		self.science_bays = science_bays
		self.sight_bays = vision_bays

	def get_module_penalty(self) -> float:
		return -(self.cargo_bays * cargo_penalty + self.science_bays * science_penalty + self.sight_bays * sight_penalty)

	def get_booster_boost(self) -> float:
		return self.boosters * booster_boost

	def get_raw_range(self, fuel: float) -> float:
		fuel /= float(fuel_scalar)
		return range_scalar * (-(fuel ** exponent) + self.oxidizer_efficiency * self.fuel_efficiency * fuel * efficiency_scalar) - self.engine_penalty

	def get_total_range(self, fuel: float) -> float:
		return self.get_raw_range(fuel) + self.get_booster_boost() + self.get_module_penalty()


def make_rocket() -> Rocket:
	return Rocket(60, 1.00, engine_penalty=0, boosters=0, cargo_bays=0, science_bays=0, vision_bays=0)


# your furthest rocket should go first!
rockets: [Rocket] = []
rockets.append(make_rocket())
rockets[-1].fuel_efficiency = 9
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 17.85 * range_scalar

rockets.append(make_rocket())
rockets[-1].fuel_efficiency = 6.5
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 9 * range_scalar

rockets.append(make_rocket())
rockets[-1].fuel_efficiency = 4
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 3.25 * range_scalar

rockets.append(make_rocket())
rockets[-1].fuel_efficiency = 2
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 0.7 * range_scalar
rockets[-1].max_fuel = 900


# rockets[3].fuel_efficiency = 2
# rockets[3].cargo_bays = 0
# rockets[3].engine_penalty = 0
# rockets[3].max_fuel = 900
# rockets[3].engine_mass = 0


def only_gets_worse(distance_delta_queue: deque) -> bool:
	a = distance_delta_queue[0]
	b = distance_delta_queue[1]
	c = distance_delta_queue[2]

	is_getting_worse = 0 > a
	if stop_graph_at_peak_range:
		is_getting_worse: bool = True

	return is_getting_worse and a > b > c


def build_legend(rocket: Rocket) -> str:
	return '[' + str(rocket.fuel_efficiency) + ',' + str(rocket.engine_penalty) + ',' + str(rocket.cargo_bays) + ']'


def main():
	max_fuel_amount = 50000
	max_distance = 0
	distance_delta_queue: deque = deque()

	for rocket in rockets:
		x_axis: [int] = []
		y_axis: [int] = []
		for fuel_amount in range(max_fuel_amount):
			if fuel_amount > rocket.max_fuel:
				break

			new_range = rocket.get_total_range(fuel_amount)

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

	# x_axis = []
	# y_axis = []
	# for i in range(max_fuel_amount):
	# 	x_axis.append(i)
	# 	y_axis.append(i * booster_boost / 400.0)
	# plt.plot(x_axis, y_axis, label="All Boosters")

	plt.xlabel('Fuel in Kg')
	plt.ylabel('Range in Km')

	pylab.legend(title='[Efficiency,EngineMass,CargoBays]', loc='upper left')

	plt.show()


main()
