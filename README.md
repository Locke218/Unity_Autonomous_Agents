#Autonomous Agents Unity Project
Author: Zane Draper

Platform: Unity 3D

Language: C# Scripting

Description:

1. A description of your world .
  Welcome to the wonderful world of 3D Pokemon. In this world, the trainer wanders the
  ‘country’ trying to catch all the pokemon. In this case, there are only 2 Pokemon available, but
  he’ll have to make due. All the assets are from the pokemon game Omega Ruby. The pokemon
  on the other had, search the country for eggs, which when picked up, replenish their ranks. I
  believe the overall concept and goals are quite clear.

2. Steering Behaviors.
  In this simulation, the Cubones and Charmanders are the flockers. They follow all the basic
  flocking behaviors ( alignment, separation and cohesion). They also follow Leader Following.
  This means that they are constantly seeking out a spot directly behind their Leader. This spot is
  along the reverse of the Forward vector. The leaders are Charmeleon and Marowak, the larger
  of the Pokemon in the scene. These Pokemon follow the Path Following behaviors. There is a
  set path they they move throughout the map. They will always seek the closest path to them.
  For both types of Pokemon, everything but separation is turned off when they are being
  pursued by a trainer. When the Path Following turns back on, they will move gradually back
  onto the path closest.

3. Character response to other characters:
  All Pokemon flee from the trainer when he is in their area. The trainer is constantly searching
  for the closest pokemon. When he finds that Pokemon he catches it. The flocking herd of
  pokemon follow their leader and after they are done being pursued, the seek their leader out
  again.

4. Character response to the environment:
  Flockers in this simulation will divert away from their leader in order to collect eggs scattered in
  the scene. These eggs then turn into more pokemon and join their flock. When being pursued
  by a trainer, the Pokemon can also move through grassy areas. These grassy areas hide them
  from the trainer. When the trainer has collected 6 Pokemon, they move to the Pokecenter in
  order to drop all their pokemon off, and then continue chasing pokemon.

5. Other notes:
  To switch between camera views, press the spacebar.

6. Resources:
  The models used in my scene where found on the Model Resource ( Link Below )
  http://www.models-resource.com/3ds/pokemonxy/

