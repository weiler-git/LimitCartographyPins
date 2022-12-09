# LimitCartographyPins

Valheim mod to limit number of pins added to cartography table.

Strongly adviced to keep a backup of your character as this mod could delete all your pins.

Features:
- Adds only explored map to cartography table by default.
- Can optionally add pins to cartography table.
- Commands to remove pins from map.

## Terminal and Slashcommands:
```
/removemypins :: Removes all personal pins from map
/removeotherspins :: Removes all pins from others from map
/removeallpins :: Removes allpins from map
/writepindata :: Enables adding pins to cartography one time
```

## Installation
Install with mod manager or manually extract into BepInEx\plugins directory.

Client side only, no need to install on server as this mod only alters how you add data to the cartogarphy table

## Note:
Valheims orignal behavior when adding discoveries to map works like this:
1) Merge map data from cartography with yours
2) Read only your pins (including pins attained from others via cartography table)
3) Save this state to cartography table

If there are pins on cartography table that you don't have, these would be deleted from table, as there is no merge function for pins.

This mod changes this behaviour:
1) Unchanged - merge map data from cartography with yours
2) Read cartograpy table pins
3) Transfer all your pins into a temporary cache
4) Remove all your pins
5) Add cartograpy table pins to your list of pins
6) Add pins from cache to your list of pins - if /writepindata has been executed, and if not duplicate with cartograpy table pins
7) Continues Valheims original method for saving your pins to cartograpy table
8) Removes all your pins
9) Transfers all cached pins back to your list of pins

Should an error occur in this process, your pins may be lost.

Always keep a backup of your character, with new patches it is reccomended to try adding discoveries to a map table, then verify that you still have your pins on your map.

In case of lost map pins, a quick alt+F4 might be enough to avoid having your character saved.

