# CC/iC Unity Tools 3D

Read Me
=======

Welcome to the [Unity](https://www.unity.com/) import and auto setup tool for **Character Creator 3**, **Character Creator 4**, **iClone 7** and **iClone 8** from [Reallusion](https://www.reallusion.com/).

This is a fully featured tool to take exports from CC/iClone and set them up in Unity with complete visual fidelity.

The tool can produce characters of the highest visual quality using custom Shadergraph shaders.
Additionally, more performance focussed characters can be produced using simpler shaders with a minimal loss of visual quality.

Progress and additional information can be found in the [Reallusion forum thread](https://forum.reallusion.com/488356/Unity-Auto-Setup).

Links
=====
[HDRP Version](https://github.com/soupday/cc3_unity_tools_HDRP)
[URP Version](https://github.com/soupday/cc3_unity_tools_URP)

Build-In Render Pipeline Support
================================

This version of the Unity auto-setup is for the Build-in render pipeline, also known as Unity 3D. To get the most accurate results the project should be set to Linear color space (found in Project settings / Player / Other Settings - Rendering / Color Space).

The preview scene for importing character materials uses the Unity Post Processing stack package (Installed via the package manager, in the Unity registry). This does __not__ need to be installed, but the preview scene looks better with it.

How it works
============

Character exports from Character Creator and iClone can be dragged into Unity, the import tool can then be opened and the character processed with a single click.

For full details of the workflow, see the [documentation](https://soupday.github.io/cc3_unity_tools/).


Obtaining the Tool
==================

The tool can be installed using Unity's internal package manager from either the Stable branch of this [git repository](https://github.com/soupday/cc3_unity_tools_HDRP) or via download of the 'Source code.zip' of the [latest release](https://github.com/soupday/cc3_unity_tools_HDRP/releases).

This process is discussed in detail in the [installation documentation](https://soupday.github.io/cc3_unity_tools/installation.html).

**Removal**

Unity's internal package manager allows the simple and safe removal of the tool.


**Updating**

Simply remove the existing tool as above and install the latest version.

