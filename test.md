
All mapping is done in Unity.

### Open the Tile Palette
Window > 2D > Tile Palette

From here you can



* Open the Scene view: At the top right of a Unity sub-window > Dropdown > Add Tab > Scene




The Map Editor will create the child GameObjects (walls, floors, doors, tables etc) in the right categories automatically and place the tiles accordingly. 

Tiles are added to the palettes as they are created. To create a new Tile follow this example:
- Choose a tile from /Tilemaps/Tiles/Objects
- Drag a tile to a palette

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

Almost never would you need to actually PR a palette change, if you do please make sure NOT to include anything other than the palette file and its .meta file.
