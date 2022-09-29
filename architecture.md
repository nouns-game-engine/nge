---
description: Describing dependencies, and implementation details of the game engine.
---

# Architecture

NounsGame has chosen C#, .NET 6.0, and FNA/Veldrid as its underlying development stack for the game engine, for the following reasons:

* C# and .NET 6.0 are open-source, can compile natively to multiple targets, and supports both WebAssembly and SIMD.
* Unity uses C# for scripting, so it is highly familiar to a large ecosystem of game developers.
* It's what the founding team knows best and where their underlying game development experience derives.
* FNA is an excellent re-implementation of Microsoft's legacy XNA Framework, with cross-platform support, including multiple consoles and experimental support for WASM/OpenGL. It has been used in countless indie games and its creator has ported dozens of titles from XNA to FNA for distribution on Linux, and consoles.
* Veldrid is a highly optimized suite of low-level tools that support modern graphics, and is an excellent candidate for PBR rendering.
