# FNA
> Modified fork maintained by While False Studios

This fork of FNA contains changes from the default FNA/XNA4 that render it incompatible with them. These include:
* The content system of XNA has been replaced with a virtual filesystem that can load assets from directories, zip files and our own 'bigfile' format (WIP). 
* Assets are no longer loaded from the xnb file format used by FNA/XNA. Textures can use any image format supported by SDL_image; models will use our own FMDL format (WIP), shaders use binary produced by either FXC or our own shader compiler (unreleased for now but functional). All 'pure data' assets now use YAML instead of binary serialisation (although we may support binary at a later time).

## Goals
* Virtual filesystem - DONE
* Bigfile - WIP
* Asset loaders for all relevant asset types - WIP
    * Texture - DONE
    * Shader/Effect - DONE
    * Model - WIP
    * Materials - not started
    * YAML - done
* Implement DeferredEngine into the core framework - not started
* [*LONG TERM*] Investigate porting rendering over to Vulkan for better portability and performance.
    * Writing backend code for Vulkan (SDL support already exists)
    * Port existing Microsoft.XNA.Framework.Graphics constructs to support Vulkan's differences from OpenGL (no on-the-fly shader modification etc)
    * Refactor effect system to use SPIR-V shaders instead of FXC and Mojoshader (remove dependency and allow for shader compilation outside of Windows/DirectX environment)

## License
FNA is released under the Microsoft Public License. See LICENSE for details.

FNA uses code from the Mono.Xna project, released under the MIT license.
See monoxna.LICENSE for details.

## Documentation
FNA changes are documented on our own wiki:

https://github.com/WhileFalseStudios/FNA/wiki

Documentation for FNA can be found on the FNA wiki:

https://github.com/FNA-XNA/FNA/wiki

## Found an issue?
Issues and patches can be reported via GitHub:

https://github.com/WhileFalseStudios/FNA/issues
