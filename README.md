# Tile_Stitching_MapGenerator
A tiny project that is a map generator that will try to make a complete map where rooms are stiched together based on the rooms given paramters

Rooms are currently made as prefabs with a few set varaibles such as the maximum dimensions of a room. that the genorator can use to see if the room still fits in the sacle of all the rooms already confirmed to be able to place.

some improvements that still need to be implemented:
- a construction design pattern so that the genorator doesn't need to place a whole prefab to see if it fits.
  preferable just a tiny footprint object so it's cheaper to check.
  
- Investigate if there's a better way to rough check against another already tile. with intent to limit the amount of tiles we actually need to precisely check for overlap.
- Investigate any possible type of checking, including height changes in a room.
