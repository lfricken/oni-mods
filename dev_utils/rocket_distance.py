
# GetRocketMaxDistance

km_per_kg_fuel = 60
kg_fuel = 813
oxydizer_efficiency = 1.33
kg_engine = 500
kg_oxy = kg_fuel

num_fuel_tanks = 1
num_oxy_tanks = 1
num_storage = 3

kg_tank = 100
kg_storage = 1000
kg_capsule = 200

weight = num_oxy_tanks * kg_tank + num_fuel_tanks * kg_tank + kg_engine + kg_fuel + kg_oxy + num_storage * kg_storage + kg_capsule
distance_penalty = max(weight, (weight/300.0)**3.2)

distance = oxydizer_efficiency * min(kg_fuel, kg_oxy) * km_per_kg_fuel

final_distance = distance - distance_penalty


print(final_distance)



