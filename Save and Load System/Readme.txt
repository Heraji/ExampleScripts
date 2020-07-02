Have one SavePointManager.cs on a gameobject of your choice per scene you want to use the save and load system.

Attach a Saveable.cs script on each object you want to be saved and loaded.

---------

You set up save points by specifying the trigger tag (for example a Player tag) and pressing the add savepoint button in the instector window of the SavePointManager. 
Then on each savepoint you can choose the type of collider you want, and adjust the size of the collider for your preferences. The game will be saved when an object with the trigger tag collides with the save points.
Load Game, New Game and Continue can be called by references or events to the public methods of the SavePointManager.  