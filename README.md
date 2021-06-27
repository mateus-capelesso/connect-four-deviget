# Connect4 - Deviget Test

This Connect4 game was made entirely based on Unity UI and events (Actions and UnityEvents) to send data from one class to another. 
There's a main Game Manager class that handles the players and the game state. The game is made to be played with two players. You can change its behaviors on the Game Manager.
The Grid class detects all the possible win conditions, which columns are available, and the best options to place the piece for the AI. 
Some UI Buttons detect the input in front of each column, so I didn't need to manage mouse events through code.

To improve the AI, I took a look at some minimax examples, but it would take a lot of time to implement it and test it, so I decided to take a straightforward path. I implemented an algorithm that tries to find a list of optimum options to place a piece. To get it, I look for the items of the same player and check their surroundings. If some of its surroundings are empty, I consider this an optimum column. Then, the algorithm picks some random elements from this list and places them on the grid.

The animations were created with DoTween.This plugin made my life easier when I had to animate some UI elements.
