Get the raw markdown for this just above this line and to the right!



All mapping is done in Unity.

### Setting Up Your Workspace
You should have the following windows open. To open a window, go to the _Window_ tab at the top of Unity.
* Window > General > __Scene__
* Window > General > __Project__
* Window > General > __Hierarchy__
* Window > General > __Inspector__
* Window > 2D > __Tile Palette__

### Introduction to Scenes
Unity uses [scenes](https://docs.unity3d.com/Manual/CreatingScenes.html) to define environments and menus. Let's look at one.
* Open an existing scene: Project > Assets > Scenes > OutpostStation
* Make sure you have your Scene window focused
* Look around the Scene with the [Hand Tool](https://docs.unity3d.com/Manual/SceneViewNavigation.html)

Notice that in the Hierarchy window selecting _OutpostStation_ selects the entire station, but not other shuttles, etc.
You can save maps and areas as [prefabs](https://docs.unity3d.com/Manual/Prefabs.html) by dragging them from the Hierarchy tab into the Project tab.

### Tile Palette
* The Tile Palette tab lets you place walls, floors, doors, tables etc. into the Scene tab
* Make sure to select the right Active Tilemap in the Tile Palette when editing
* The new tiles and objects will be added to the right categories within the active tilemap automatically

### Edit a Palette
Tiles are added to the palettes as they are created. To create a new Tile follow this example:
- Choose a tile from /Tilemaps/Tiles/Objects
- Drag a tile to a palette

### Create a new map
* TODO: How do you create a new map from a station palette?
* TODO: How do you add your new map for map selection, other than this: [Testing custom maps in Multiplayer](https://github.com/unitystation/unitystation/wiki/Building-And-Testing#testing-custom-maps-in-multiplayer)?

## Object-Specific Mapping Info
This section describes the specifics of how various objects are mapped (anything that isn't obvious from within the editor).
### Wallmounts
Wallmounts always appear upright, but you need to indicate which side of the wall they are on (they should only be visible from one side of a wall). To do this, you must adjust their transform rotation around the z axis so that their rotation points in the direction that they are facing. For example, on Outpost station, the following Rotation Z values correspond to the following directions:
* 0 - facing down
* 90 (or -270) - facing right
* 180 (or -180) - facing up
* -90 (or 270) - facing left

The sprite will still appear upright, but in game you should see that it disappears when you are on the wrong side of the wall.

The sprite rotation inside of the wallmount will have no effect in game even if it makes the wallmount appear rotated in the editor - it is always set based on script logic. 

## Chairs
Chairs must be rotated by setting their sprite manually to one of their 4 chair sprites. Make sure the sprite set in the sprite renderer shows up in the Chair component's list of sprites. Do not try to rotate chairs by adjusting their transform rotation. Just change their sprite.

## Pull Requests for Tile Palette Changes
Almost never would you need to actually PR a palette change, if you do please make sure __NOT__ to include anything other than the palette file and its .meta file.

