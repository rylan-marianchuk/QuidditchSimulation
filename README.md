# QuidditchSimulation
Computer Science 565 Project

Unity is an engine tuned for multi-agent simulations. In this project I attempt to model the game of Quidditch from the Harry Potter series. [1]
The reduction and simplification of graphics here is substantial. I instead focus on player behavioral properties.

# Game Outline

Players for Team 0 (Slytherin) and Team 1 (Gryffindor) begin at a predefined spawn location on either side of the arena which is determined by using Random.onUnitSphere. Players follow their algorithmic movement behaviour, parameterized by their trait assignment (see below) in hopes to colliding with the golden Snitch. 

Team that capture the Snitch obtain a score, with sucessive scores worth 2 points. The snitch randomly respawns after capture and continues its random perturbations floating throughout the arena.

First team to 100 points wins the game.


# Player Traits

Much like the distribution of evolutionary strategies that give rise to human individual variance at the biological level, each player recieves a degree of the following traits according to a parameterized normal distribution. The given traits are as follows:

  - Weight
  - Max Velocity
  - Aggressiveness
  - Max Exhaustion

# Uniquely Implemented Behviours


# Emergent Properties of Players


# References
1. https://harrypotter.fandom.com/wiki/Quidditch
