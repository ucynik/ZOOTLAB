### ZOOTLAB
ZOOTLAB is a Unity suite for creating and simulating Arknights maps. Currently, only enemies are simulated but it is planned for operators to be implemented in the future.

ZOOTLAB is a fan-made, unofficial hobby project for Arknights. This is not affiliated with Hypergryph or Yostar.

### Requirements
- Unity 2021.3.29f1. Newer versions may be compatible, but it remains untested
- Blender or any 3D modeling software that can work with *.fbx files
- A program (that supports LZHAM decompression) to unpack *.dat or *.ab files if necessary
  - ArkUnpacker: https://github.com/isHarryh/Ark-Unpacker
  - AssetStudioGUI (Arknights Modification): https://github.com/aelurum/AssetStudio/releases/tag/ak-v1.2.3 

### Getting Started

## Retrieving Assets
Arknight's assets are stored within *.ab files. To access these files, you must root an Android emulator.
Using LDPlayer9, the game files for the global version of Arknights are located under: ./storage/emulated/0/Android/data/com.YoStarEN.Arknights/.

Within the folders you will find *.ab files which can be extracted using the aforementioned decompression tools. 
An important thing to note: post-release of Chapter 15, HG uses LZHAM compression for their files, which require decompression tools that support LZHAM. If you are working with a copy of AK files pre-Chapter 15, you may use other programs, such as the normal version of AssetStudio: https://github.com/Perfare/AssetStudio 
