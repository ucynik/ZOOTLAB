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

Upon finding your desired stage, extract the scene and you should find an `*.obj` file, typically named `Combined Mesh (root_ scene).obj`. You will then need to import this into your 3D modeling program or Unity. The map creation process using the scene objects should be fairly straightforward, consisting of copy and paste and transformation operations. I recommend enabling grid snapping if your program allows, to keep tile positions and orientation uniform. When you are done, export your map as an `*.fbx` file and import it to Blender. It is your preference whether you apply textures to your map in Unity or Blender.


