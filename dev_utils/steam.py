# Created by lfricken https://github.com/lfricken/oni-mods
# Calculates rocket distances.
# equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance

import math
import random

import matplotlib.pyplot as plt
import pylab
from collections import deque

def roll_die(max_value):
	return random.randint(1, 6) <= max_value

a = [0, 0, 0, 0, 0, 0]

def get_num_rolls() -> int:
	for i in range(1,7):
		if roll_die(i):
			return i

def main():

	total = 0
	tries = 30000
	for i in range(tries):
		#print(get_num_rolls())
		total += get_num_rolls()

	print (total/tries)

	total = 0
	p_will_roll = 1.0
	for i in range(1,7):
		print ("p roll" + str(p_will_roll))
		odds = ((6.0 - i)/6.0)
		odds_win_now = (i/6.0)
		total += p_will_roll * i * odds_win_now
		p_will_roll *= odds


	print (total)

main()
