---
description: Discussing hyperstructures and the extent they are employed in NounsGame.
---

# Hyperstructures

{% embed url="https://jacob.energy/hyperstructures.html" %}
The nomenclature for _hyperstructures_ is defined in this article by Jacob @ Zora
{% endembed %}

## Approach & Caveats

NounsGame is a game engine. To succeed as a hyperstructure, it must be [unstoppable](<README (1).md>). Game engines are inherently "living" codebases, because of the shifting capabilities of user runtime environments, graphics processing standards and devices, and the state of library and hardware support across a wide variety of concerns.

While the primary quality of unstoppability is immutable code execution, for gaming concerns it must also include determinism. Determinism in this case means predictable output from game subsystems, such as graphics rendering and physics simulation, regardless of platform.&#x20;

One aspect of determinism is fixed-point math, which removes some vectors for instability where different machine architectures will de-synchronize over time. De-synchronization is a major concern for game networking, as multiple players in a game rely on a shared state to predict where other players and game objects are in a game simulation and where they should end up based on physical forces at play.

EVM does not yet support floating-point math, and so this constraint for determinism is achieved by default, though it takes more effort to properly develop and test math libraries in fixed-point.&#x20;

To achieve determinism and safe-guard unstoppability, NounsGame must use Ethereum Virtual Machine (EVM) native code execution in as many sub-systems as possible, and particularly in the graphics and physics realms.

This is difficult due to the following constraints:

* The EVM is a stack machine, which is challenging to parallelize, as operations rely on shared memory whose manipulations must come in a specific order, as the values pulled from the stack for operations are implicit. Executing code in parallel is a key component of performance optimization, but is also important for coordinating the work of multiple gaming sub-systems, such as blackboard architectures for AI, and hyper-simulation of game state for predictive networking design.
* EVM graphics, such as the [compositing libraries](https://github.com/kohiart/kohi-comopser) developed by [Kohi Art Community](https://kohi.art), is software-oriented, implemented with a scanline-based rasterizer. GPUs' in-built graphics rendering is oriented around drawing triangles. An approach using compute shaders (GPGPU) and binning algorithms is likely needed to run EVM-based graphics processing at a scale suitable for gaming and realtime artworks.

It's the opinion of the NounsGame project that it is not sufficient to run third-party graphics or physics libraries, as these are a vector for continuous change, and the primary way a game engine hyperstructure could degrade, making it no longer unstoppable, as deployed code could simply stop working over a long enough time horizon without direct intervention by a project owner or contributor.

Even in the case where EVM code can run in a graphic accelerated context, the means by which that code is instrumented to run on a GPU or other appliance, through shaders or other means, must itself be unstoppable. For this, we could choose a code-generated approach with multiple targets. Though it is unlikely to ever avoid switching out the execution engine on a long enough time scale, we can use the predictable, deterministic EVM code as the singular execution model, and limit the amount of change to processes that execute it. In this way, we will at least guarantee that _something_ can always execute the code (i.e. the blockchain client's EVM implementation) at minimum, even if it does not exhibit the performance characteristics we want. In extreme cases, pre-processing or caching mechanisms can address static assets that are used in game pipelines.



