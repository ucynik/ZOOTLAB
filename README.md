# ZOOTLAB
ZOOTLAB is a Unity suite for creating and simulating Arknights maps. Currently, only enemies are simulated but it is planned for operators to be implemented in the future.

ZOOTLAB is a fan-made, unofficial hobby project for Arknights. This is not affiliated with Hypergryph or Yostar.

# Requirements
- Unity 2021.3.29f1. Newer versions may be compatible, but it remains untested.
- Blender or any 3D modeling software that can work with `*.obj` and `*.fbx` files, as well as basic knowledge utilizing those programs.
- A program (that supports LZHAM decompression) to unpack `*.dat` or `*.ab` files if necessary:
  - [ArkUnpacker](https://github.com/isHarryh/Ark-Unpacker)
  - [AssetStudioGUI (Arknights Modification)](https://github.com/aelurum/AssetStudio/releases/tag/ak-v1.2.3 )

# Getting Started

## Retrieving Assets
Arknight's assets are stored within `*.ab` files. To access these files, you must root an Android emulator.
Using LDPlayer9, the game files for the global version of Arknights are located under: `.\storage\emulated\0\Android\data\com.YoStarEN.Arknights`.

Within the folders you will find `*.ab` files which can be extracted using the aforementioned decompression tools. 
An important thing to note: post-release of Chapter 15, HG uses LZHAM compression for their files, which require decompression tools that support LZHAM. If you are working with a copy of AK files pre-Chapter 15, you may use other programs, such as the normal version of [AssetStudio](https://github.com/Perfare/AssetStudio ).

Currently, there is an issue where some game files do not appear anywhere despite their assets being used in-game, this is still being looked into. I an willing to provide files on the Discord server if I have them.

## Creating Your First Map
For this section, you will need a 3D modeling program that works with `*.obj` or `*.fbx*` files.

My personal workflow takes a map(s) and edits it to my liking within a 3D modeling program, there is no singular way to approach map creation. Using a 3D modeling program is optional if you are more comfortable assembling your map in Unity. I choose to use Blender for my own convenience.

Within `.\com.YoStarEN.Arknights\files\AB\Android\scenes` are folders containing 'scene' assets which are essentially the map assets that battles take place on.
- `.\scenes\activities` stores side story and vignette event stages, named after their order of appearance
- `.\scenes\obts` stores all other stages

Within `.\com.YoStarEN.Arknights\files\AB\Android\arts\maps` are folders containing textures for various tile sets.

Upon finding your desired stage, extract the scene and you should find an `*.obj` file, typically named `Combined Mesh (root_ scene).obj`. You will then need to import this into your 3D modeling program or Unity. The map creation process using the scene objects should be fairly straightforward, consisting of copy and paste and transformation operations.

I recommend enabling grid snapping if your program allows, to keep tile positions and orientation uniform. When you are finished, export your map as an `*.fbx` file and import it to Unity. It is your preference whether you apply textures to your map within Unity or Blender.

## Using Unity

### Setting Up The Map

Open the provided `ZOOTLAB.unitypackage` in Unity. You will be greeted by the `.\Assets` directory. Enter the `Map` directory and drop your exported map file in here.

The Hierarchy contains multiple GameObjects, each serving a purpose:
- `ExampleMap`: The map asset.
- `Enemies`: The enemies to be spawned in. They sit within the scene so that the enemy spawner may clone these GameObjects.
- `Utility`: The camera, lights, and audio utility. The lights here are for illuminating `ExampleMap`; modify these to best fit your map. Within `Utility` is `Default`, which contains the camera as well as several post-processing components.
- `Waypoints`: The paths and spawners for enemies.
- Everything else in the example scenes are incursion points, objective points, and an active Originium tile.

Drag your map into the scene, and set the transform to `<0,0,0>` and the rotation to match that of `ExampleMap` so that the ground points up in the `Y Axis`. You may delete or hide `ExampleMap` in the Hierarchy.
If all goes well, switching to the `Game` view should show your map from the perspective of the `Default` camera, mimicking the battle view in Arknights.

The `Utility` directory under `.\Assets` contain various assets that may be useful, such as stage features and status icons. Objective and incursion points can be found here, as well as holes and active Originium tiles. Drag these into the scene and position them where you see fit in order to use them.

### Setting Up Enemy Assets Pt. 1

Arknights' enemy models are Spine2D models that consist of a texture (`*.png`), Skeleton (`*.skel`), and Atlas (`*.atlas`) file. These files can be found within the game files but for a simpler method, I recommend using the [PRTS Wiki](https://prts.wiki/w/%E9%A6%96%E9%A1%B5).

Using your browser, head over to the PRTS Wiki and open the Inspector (typically bound to `F12`). Using the [Arknights Terra Wiki](https://arknights.wiki.gg/), find the enemy you want and search for the enemy `Code`.

Take the `Code` and use it to find that enemy's wiki page on PRTS. Scroll to the bottom and open the enemy model viewer (敌人模型). Within `Network` in the Inspector, you will notice 3 files appear named `enemy_****.png/skel/atlas`. Double click on these to download the three aforementioned files that an enemy model consists of.

Before importing these to Unity, you will need to:
- Append `.txt` to the Atlas file.
- Append `.json` to the Skeleton file.
  
Remember to **append** these file extensions to the existing one, not replace them with.

In Unity, enter the `Enemies` directory and create a folder to contain the three files. Drag the three files into the folder you just created and the enemy's Spine2D model should generate the following files:
- A `*.asset` SkeletonData file.
- A `*.mat` material file.

### Setting Up Enemy Assets Pt. 2

In Unity, open `Enemies` in the Hierarchy and find an arbitrary enemy asset. The Inspector will show a `SkeletonAnimation` component attached to the asset. In the `Mesh Renderer` apply your enemy's `material` here (you can drag and drop), then apply your enemy's `SkeletonData` in their respective attributes. Press `Reload` under the `SkeletonAnimation` component and your enemy's model should appear.

Make sure to set animations in the `Animate` component if applicable. Animations for a model can be viewed in the `Animation Name` dropdown under the `SkeletonAnimation` component. If your enemy will use an animation, it must be set within teh `Animate` componenty or else it will not function and throw an error.

To preview the enemy in the Scene tab, double click a few times to go to your enemy in the world, and enable their `Mesh Renderer` component to view them. Under the `Transform`, you may edit the scale to a size you think is appropriate (you literally have to eye it because the method I use to find exact enemy scales doesn't work anymore). The scale of enemies typically lies on a rage of `0.2 to 0.35`.

Familiarize yourself with the other components on the enemy asset. Other than that, you are finished with this step!

### Setting up Waypoints

Thankfully, pathing is much simpler than it seems to be.

Within `Waypoints` in the Hierarchy you will find several GameObjects with names corresponding to single example enemies. Each GameObject contains several children which are the individual points an enemy can travel to.

Clicking on the parent GameObject will reveal a `Spawner` component attached to it which contains the following attributes:
- `Spawn Point`: The transform of the spawn point of an enemy, typically derived from the parent GameObject.
- `Enemy`: The enemy asset to be spawned.
- `Hui/Ming`: Hui/Ming attributes if needed.
- `Spawn Times`: The points in time, in seconds, of which the `Enemy` will be spawned.
- `Waypoints`: The waypoints, in order, that an instance of the `Enemy` will travel to. Each waypoints contain the following attributes:
  - `WP Transform`: The transform of the waypoint, derived from the Waypoint GameObject.
  - `Stop`: The duration, in seconds, the enemy will rest for upon reaching this waypoint.
  - `Phase`: A flag that tells an enemy to transition phases upon reaching this waypoint.
  - `Stairs`: A flag that tells an enemy to teleport to the next waypoint.
  - `Stairs Duration`: The duration, in seconds, the enemy will wait before appearing at the next waypoint.
  - `New 'x'`: A new animation the enemy will use after reaching this waypoint. If an enemy has new animations after transitioning phases, set them in the same waypoint that `Phase` is ticked.

To set up pathing for your enemy, simply fill out the appropriate attributes according to your needs. If done correctly, your enemy will travel along the paths you set, and disappear when they reach your endpoints.

# You're Done!

Hopefully you were able to cram all that into your head. Your map should have basic functionality at this point and you can view it play out by pressing the play button at the top middle of the view window. To record, the Recorder package for Unity works quite nicely.

This guide served as a basic set of instructions to get a working map down, and will be expanded as more features are implemented. As for specific instructions for replicating Arknights' maps' aesthetic effects, perhaps I will make a guide for that in the future. Thank you for giving ZOOTLAB a try. 









