# LimitCartographyPins

Valheim plugin to limit number of pins added to cartography table.

Strongly adviced to keep a backup of your character as this mod could potentially delete all your pins, be extra careful after game patches.

Features:
- Adds only explored map and boss locations to cartography table by default.
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

## Plugin Details:
### Valheims new orignal behavior when adding discoveries to map works like this:

When clicking Read on map table:
- Merge explored map into yours
- Delete others pins from your map
- Import pins from table
- Skip importing pins close to existing pins
- Skip importing pins created by yourself

When clicking Write on map table:
- Read map table into your own map as if you click read (inlcuding delete others pins from your own map)
- Merge with your map
- Read map again from table
- Merge your map with table map into package
- Add your pins into package
- Send to Owner/Host over RPC

### This mod changes to this behaviour:

When reading map:
- Skip deleting others pins from your map
- Get all pins from the map table, including those you have made yourself
>Slash commands allows you to delete pins from your map.
>Map table can be rebuild if you want to erase all records.

When writing to map:
- Skip the read operation and bypass most of the original merge code
- Read the tables data, and merge explored map with our own.
- Read all pins on the table, merge with our own pins.
- Player Pins (the pins players can make themselves), will be skipped by default.
- Duplicate pins will be skipped.
- The merged data will be sent by RPC to Owner/Area Host
>Slash command allows you to add Player Pins to the map
 
### Why have it like this:
- We have a public map that everyone can read from, with a designated character adding pins for various world locations.
- Teams have their own map, can share pins as they wish, and can read pins from public table without removing the teams pins and vice versa.
- Players might want their own map table to share between alts.

## Changelog

1.1.0 Reworked from ground up as Valheim have made breaking changes, made safer approach to avoid risk of losing pins.

1.0.4 Fixed error when writing more than once.

1.0.3 Recompiled for Valheim V 0.217.24

1.0.2 Recompiled for Hildirs patch.

1.0.1 Fixed Null error when updating blank cartography table.

1.0.0 Initial release.
