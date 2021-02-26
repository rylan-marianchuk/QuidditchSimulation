# QuidditchSimulation
## Computer Science 565 Project

Unity is an engine tuned for multi-agent simulations. In this project I attempt to model the game of Quidditch from the Harry Potter series. [1]
The reduction and simplification of graphics here is substantial. I instead focus on player behavioral properties.

# Game Outline

Players for Team 0 (Slytherin) and Team 1 (Gryffindor) begin at a predefined spawn location on either side of the arena which is determined by using Random.onUnitSphere. Players follow their algorithmic movement behaviour, parameterized by their trait assignment (see below and [2]) in hopes to colliding with the golden Snitch. 

Players that collide with other players, the arena, or reach max exhaustion go unconscious (turn red) and fall to the ground, then respawn at their location.

The team that captures the Snitch obtain a +1 score, with sucessive scores worth 2 points. The snitch randomly respawns after capture and continues its random perturbations floating throughout the arena.

First team to 100 points wins the game.


# Player Traits

Much like the distribution of evolutionary strategies that give rise to human individual variance at the biological level, each player recieves a degree of the following traits according to a parameterized normal distribution. The given traits are as follows:

  - Weight
  - Max Velocity
  - Aggressiveness
  - Max Exhaustion

Players current exhaustion gets updated probablistically according to an exponential distribution. Every frame a sample is taken from Exp(\lambda) and if it passes a predefined threshold, current exhausted is set to its velocity * weight.

In order to ensure players don't continuously go unconscious I use a velocity damper. If current exhaustion >= aggressiveness, then velocity is dampened by 0.99.

# Uniquely Implemented Behviours
- Joker: Binary trait. Each team has a low chance (between 5% and 15%) chance of having each player be a joker. A Joker isn't actually attracted to the snitch, instead its goal it to destory the player on opposing team closest to the snitch. As such it is given high aggression and low weight so it moves quick.
- Urge: Players no longer care about preserving exhaustion levels when very close to the snitch. Instead they are urged with greater speed to score. 20% of players have this trait.

# Emergent Properties of Players


# References
1. https://harrypotter.fandom.com/wiki/Quidditch
2. https://github.com/omaddam/Boids-Simulation
