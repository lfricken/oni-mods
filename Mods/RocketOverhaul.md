
# Rocket Overhaul

## About
The normal ONI rocket equation means that the rocket with the best fuel is always the rocket you should use, and that [Oxylite](https://oxygennotincluded.gamepedia.com/Oxylite) is useless after you can manufacture [Liquid Oxygen](https://oxygennotincluded.gamepedia.com/Liquid_Oxygen). This mod:
* Adds a new Methane Engine which is even more powerful than the [Hydrogen Engine](https://oxygennotincluded.gamepedia.com/Hydrogen_Engine).
* New equations mean you build huge rockets with great return on investment.
* Makes Boosters and Module range boosts and penalties linear, so the calculation is easier to understand.
* The first Booster provides +30,000km, then +20,000km, +10,000km, and then none.
* Cargo Bay: -30,000km
* Research Module: -8,000km
* Sightseeing Module: -2,000km
* Gives a reason to use different engines under different circumstances.
* Mixing [Oxylite](https://oxygennotincluded.gamepedia.com/Oxylite) _and_ [Liquid Oxygen](https://oxygennotincluded.gamepedia.com/Liquid_Oxygen) equally gives the largest boost to efficiency, providing a reason to make both.
* Makes it possible for the best rockets to return more mass via [Cargo Bays](https://oxygennotincluded.gamepedia.com/Cargo_Bay) then it costs to launch it.
* Makes Boosters useful instead of [useless](https://forums.kleientertainment.com/forums/topic/97074-solid-booster-useless-solved/).  (For most rockets a full Booster actually reduces range)


RocketOverhaul.dll


## Notes
Technically, this is less realistic than the games default equations, but that's ok. If you want a super realistic game, go play real life, (or KSP). The purpose of some games is to provide more [[interesting, fun, achievable, varied]] challenges than you can find in real life. Sometimes, in order to achieve that you need to modify the fundamental equations of reality, in this case, how are rockets propelled. For example, KSP shrinks the size of planets by 10x!

This new equation is unrealistic because it mostly ignores [Tsiolkovsky's Rocket Equation](https://en.wikipedia.org/wiki/Tsiolkovsky_rocket_equation) and instead opts for a combination of linear and quadratic equations to compute rocket distance. You can think of the number of intersections between lines in the range graphs as points where one strategy starts becoming better than another. Examining both graphs, its easy to see that the default equation has no intersection points. The new equation has many.

You might argue that my plots don't take into account the relative cost of manufacturing the fuel, which is true. But that is very hard to calculate, and from playing the game myself, once the infrastructure was up and running, the costs were relatively low.

## Old Equation
* Notice a [Hydrogen Engine](https://oxygennotincluded.gamepedia.com/Hydrogen_Engine) is the best rocket no matter what?

![rocket efficiency graph](/images/rocket_distance.png "The green line is the rocket with your mom on it.")

## New Equation
* Notice the _best_ rocket depends on how much you want to invest.
* [New equation in python here](/dev_utils#rocket-overhaul-mod-distance-rocket_distance_overhaulpy).

![new rocket efficiency graph](/images/new_equation.png "Just kidding about the rocket with your mom on it. That rocket was never built due to budget concerns.")

## New Starscreen (new vs old)
![new_starscreen](/images/compare_screen.png "No more jokes. Download my mod.")
