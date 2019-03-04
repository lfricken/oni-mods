# Created by lfricken https://github.com/lfricken/oni-mods
# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import matplotlib.pyplot as plt
import pylab
import copy
from collections import deque

stop_graph_at_peak_range: bool = True

# https://www.desmos.com/calculator/htfdxbgqdt
# https://www.wolframalpha.com/
#
# p = -0.075
# (h,k)
#
# Vertex: (1,1)
# y = -3.33333 x^2 + 6.66667 x - 2.33333
#
# Vertex: (2,2.5)
# y = -3.33333 x^2 + 13.3333 x - 10.8333
#
# Vertex: (3.25,5)
# y = -3.33333 x^2 + 21.6667 x - 30.2083
#
# Vertex: (4.5,8)
# y = -3.33333 x^2 + 30.0 x - 59.5

# multiplied by whole equation
parabolic_extraction = 3.333

# multiplied by x
r1_base_efficiency = 2
r2_base_efficiency = 4
r3_base_efficiency = 6.5
r4_base_efficiency = 9

# multiplied by constant
r1_base_range = 0.7
r2_base_range = 3.25
r3_base_range = 9
r4_base_range = 17.85

# convert to real life exhaust velocities
real_hydrolox = 4423
oni_hydrolox = 6.5

# range penalties
cargo_penalty = 30000
science_penalty = 12000
sight_penalty = 5000

booster_boost_ranges = [30000, 20000, 10000, 0]
fuel_per_booster = 400

# range equation
miles_per_y_value = 40000.0
exponent = 2.0
fuel_per_x_value = 900.0  # fuel per int
efficiency_scalar = oni_hydrolox / real_hydrolox
range_scalar = parabolic_extraction * miles_per_y_value


class Rocket:
	def __init__(self, fuel_efficiency, oxidizer_efficiency, engine_penalty, boosters=0, cargo_bays=0, science_bays=0, vision_bays=0):
		self.name = ""

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

	@staticmethod
	def get_booster_boost(booster_fuel_total: float) -> float:
		if booster_fuel_total < 0:
			return 0

		remaining_booster = booster_fuel_total / fuel_per_booster
		total_range = 0
		for i in range(min(len(booster_boost_ranges), math.ceil(remaining_booster))):
			if remaining_booster >= 1:
				total_range += 1 * booster_boost_ranges[i]
			else:
				total_range += remaining_booster * booster_boost_ranges[i]
			remaining_booster -= 1

		return total_range

	def get_raw_range(self, fuel: float) -> float:
		fuel /= float(fuel_per_x_value)
		return range_scalar * (-(fuel ** exponent) + self.fuel_efficiency * fuel * efficiency_scalar) - self.engine_penalty

	def get_total_range(self, fuel: float) -> float:
		return self.oxidizer_efficiency * self.get_raw_range(fuel) + self.get_booster_boost(self.boosters * fuel_per_booster) + self.get_module_penalty()


def make_rocket() -> Rocket:
	return Rocket(60, 1.2, engine_penalty=0, boosters=0, cargo_bays=0, science_bays=0, vision_bays=0)


# your furthest rocket should go first!
rockets: [Rocket] = []
rockets.append(make_rocket())
rockets[-1].name = "Methane"
rockets[-1].fuel_efficiency = 6127
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 2380000

rockets.append(make_rocket())
rockets[-1].name = "Hydrogen"
rockets[-1].fuel_efficiency = 4423
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 1200000

rockets.append(make_rocket())
rockets[-1].name = "Petroleum"
rockets[-1].fuel_efficiency = 2721
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 435000

rockets.append(make_rocket())
rockets[-1].name = "Steam"
rockets[-1].fuel_efficiency = 760
rockets[-1].cargo_bays = 0
rockets[-1].engine_penalty = 20000
rockets[-1].max_fuel = 515


def only_gets_worse(distance_delta_queue: deque) -> bool:
	a = distance_delta_queue[0]
	b = distance_delta_queue[1]
	c = distance_delta_queue[2]

	is_getting_worse = 0 > a
	if stop_graph_at_peak_range:
		is_getting_worse: bool = True

	return is_getting_worse and a > b > c


def build_legend(rocket: Rocket) -> str:
	return '[' + str(rocket.name) + ']'


def main():
	max_fuel_amount = 50000
	max_distance = 0

	peaks: [(int, int)] = []

	for rocket in rockets:
		distance_delta_queue: deque = deque()
		peaked = False
		x_axis: [int] = []
		y_axis: [int] = []
		for fuel_amount in range(max_fuel_amount):
			if fuel_amount > rocket.max_fuel:
				break

			new_range = rocket.get_total_range(fuel_amount)

			distance_delta_queue.append(new_range)
			if len(distance_delta_queue) >= 3:
				if only_gets_worse(distance_delta_queue) and not peaked:
					peaks.append((fuel_amount, new_range))
					peaked = True
					if max_distance == 0:
						max_distance = new_range
						max_fuel_amount = fuel_amount
						break  # rocket distance will never go up again

				distance_delta_queue.popleft()

			x_axis.append(fuel_amount)
			y_axis.append(max(float(0), new_range))

		if len(x_axis) == 0:
			print('Your rocket was so bad that it cant even get 1 km of range! Try improving fuel efficiency or reducing weight.')

		plt.plot(x_axis, y_axis, label=build_legend(rocket))

	# add boosters to graph
	x_axis = []
	y_axis = []
	for start in reversed(peaks):
		x_axis = []
		y_axis = []
		for i in range(fuel_per_booster * 3):
			x_axis.append(i + start[0])
			y_axis.append(Rocket.get_booster_boost(i) + start[1])
		plt.plot(x_axis, y_axis, label="+3 Boosters")

	# draw ROI
	x_axis2 = []
	y_axis2 = []
	for i in range(x_axis[-1]):
		x_axis2.append(i)
		y_axis2.append(cargo_penalty / 500 * i)
	plt.plot(x_axis2, y_axis2, label="Break Even Fuel+Oxy vs Cargo")

	plt.xlabel('Fuel in Kg')
	plt.ylabel('Range in Km')

	pylab.legend(title='Legend', loc='upper left')

	plt.show()


main()
