---
description: Describing dependencies, and implementation details of the game engine.
---

# Architecture

NounsGame has chosen [C#](https://learn.microsoft.com/en-us/dotnet/csharp/), [.NET](https://dotnet.microsoft.com/en-us/), and [FNA](https://github.com/FNA-XNA/FNA)/[Veldrid ](https://github.com/mellinoe/veldrid)as its underlying development stack for the game engine, for the following reasons:

* C# and .NET are open-source, can compile natively to multiple targets, and supports both [WebAssembly](https://webassembly.org/) (WASM) and [SIMD](https://en.wikipedia.org/wiki/Single\_instruction,\_multiple\_data).
* Unity uses C# for scripting, so it is highly familiar to a large ecosystem of game developers.
* It's what the founding team knows best and where their underlying game development experience derives.
* FNA is an excellent re-implementation of Microsoft's legacy [XNA Framework](https://en.wikipedia.org/wiki/Microsoft\_XNA), with cross-platform support, including multiple consoles and experimental support for WASM/[WebGL](https://get.webgl.org/). It has been used in countless indie games and its creator has ported [dozens of titles](https://www.flibitijibibo.com/index.php?page=Portfolio/Tools#01\_FNA.txt) from XNA to FNA for distribution on Linux, and consoles.
* Veldrid is a highly optimized suite of low-level tools that support modern graphics, and is an excellent candidate for [physically-based rendering](https://en.wikipedia.org/wiki/Physically\_based\_rendering) (PBR).
