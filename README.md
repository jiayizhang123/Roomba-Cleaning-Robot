# Roomba-Cleaning-Robot
This project demonstrates a Roomba cleaning robot cleaning a room in whole. This program implements the NavMesh surface, NavMeshObstacle, NavMeshAgent and related AI for pathfinding. There are preset obstacles and moving human in the room. When program starts, the cleaning robot will calculate the size of room and make a grid according to the room's size and position, and set a corner as its starting point for cleaning, then use NAV agent to avoid the obstacles in the path when moving, then cleaning the room line by line in the grid until the whole room is cleaned.
![alt text](https://github.com/jiayizhang123/Roomba-Cleaning-Robot/blob/main/demo2.png?raw=true)

The gizmos of unity is used for debugging to show the cleaning robot's moving direction by red line, and draw cubes to show the grid, green ones represent the target and cleaned ones , and yellow ones are the rest.
![alt text](https://github.com/jiayizhang123/Roomba-Cleaning-Robot/blob/main/demo1.png?raw=true)
