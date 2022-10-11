# Year One

In the first year, core level infrastructure is defined and developed, and two example games are produced to showcase the potential of the engine and deliver on the promise of "an infinite meme platform" through modifiable, distributable _Nounish_ games.

### Game Delivery

To showcase the engine features, deliver on meme proliferation, and provide well-architected examples for further modification, we will ship two simple but fully articulated example games.

#### Platformer Game

A platformer game demonstrating the 2D pixel-based engine.

#### Brawler Game

A fighting/brawler game demonstrating the 2.5D pixel-based engine.

### Component/Feature Delivery

Success of the project depends on these component deliverables, the absence of which blocks use of the engine as a public good.

* **Engine Modes**
  * Pixel 2D engine with 1D inelastic physics
  * Pixel 2.5D engine w/ 3D physics
* **Camera Subsystem**
  * UI overlay
  * [Culling](https://en.wikipedia.org/wiki/Occlusion)/[Scissoring](https://www.khronos.org/opengl/wiki/Scissor\_Test)/[Stencils](https://en.wikipedia.org/wiki/Stencil\_buffer)
  * Post-Production Hookups
* **Audio Subsystem**
  * Cues/Triggers
  * Spatial Audio
* **Input Subsystem**
  * Keyboard
  * Gamepads
  * Mouse
  * Input mapping
* **Asset Pipeline**
  * Live Asset Rebuild
  * Asset Bundling (for First Time Experience (FTE) and startup speed)
* **In-Game Editor (**[**IMGUI**](https://en.wikipedia.org/wiki/Immediate\_mode\_GUI)**-based)**
  * Editing Snap-Ins
  * Snapshots/Loop Recording
  * Drag-And-Drop Importing
  * Scene/Stage/Levels
* **Configuration Subsystem**
  * File-Based ([TOML](https://toml.io/en/))
  * Two-Way Binding
* **State Machine**
  * Scripting
* **Game State Serializer**
  * Network Safety
  * Rollups ([EIP-5050](https://github.com/ethereum/EIPs/blob/master/EIPS/eip-5050.md) or equivalent)
* **Localization Subsystem**
  * Symbol Editor
* **Crash Reporting**
  * Stack Dumps
  * Structured Logging
  * Snapshots
* **Runtime Subsystem**
  * Level [Streaming](https://en.wikipedia.org/wiki/Streaming\_media)
  * Audio Streaming
* **Networking Subsystem**
  * Network "Ready"
  * Smoothing/Prediction Hookups
* **Distribution Subsystem**
  * _Auto-Generated Builds_
    * Windows
    * macOS
    * Linux
  * _EVM Deployment_
    * Shared Contracts/Libraries
    * Game Registry
    * Front-End



