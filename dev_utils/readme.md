### About
* This dev_utils folder provides python files for calculating rocket distances.
* Each file calculates and displays a graph of fuel vs range.

### Rocket Overhaul Mod Distance: rocket_distance_overhaul.py
* Uses a new equation to calculate rocket distance.
* Check out my [Rocket Overhaul Mod](/Mods/RocketOverhaul.md)!
* [View Source](/dev_utils/rocket_distance_overhaul.py)

![new rocket efficiency graph](/images/new_equation.png "Just kidding about the rocket with your mom on it. The rocket was never built due to budget concerns.")

### Rocket Distance: rocket_distance.py
* Graphs a rocket's distance with any specified fuel efficiency. You can easily modify anything at the top of the file. 
* If you want to know whether your new rocket engine mod would be OP, look no further.
* Useful for playing with rocket mechanics: Fuel Efficiency, Weight, etc.
* Requires Python 3.6+
* Equations obtained from Assembly-CSharp.RocketStats.GetRocketMaxDistance 
* Check out [oni-assistant](https://oni-assistant.com/). It's a web app version of this!
* [View Source](/dev_utils/rocket_distance.py)

![rocket efficiency graph](/images/rocket_distance.png "The green line is the rocket with your mom on it.")
