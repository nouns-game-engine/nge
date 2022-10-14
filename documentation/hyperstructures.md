---
description: >-
  Discussing hyperstructures and the extent they are employed in Nouns Game
  Engine.
---

# Hyperstructures

### What is a Hyperstructure?

A term coined by [Jacob @ Zora](https://twitter.com/js\_horne), a hyperstructure is a [description of a software system](https://jacob.energy/hyperstructures.html) with specific properties. in Nouns Game Engine terms, these properties are:

#### Unstoppable

> The protocol cannot be stopped by anyone. It runs for as long as the underlying blockchain exists.

For Nouns Game Engine, this is the most important aspect of a hyperstructure. A game engine is a collection of ever-evolving parts. To achieve unstoppability, it must run on the native execution layer of its host blockchain, in this case Ethereum's EVM.&#x20;

This is achievable through a gradient of processes, including deterministic graphics rasterization and physics simulation, storage and encoding of assets, runtime code generation of client-side backends, and component interaction with the blockchain network.

However, much like the variety of available Ethereum clients are under continuous change, so too must Nouns Game Engine, itself a custom Ethereum, change over time to address correctness, and security concerns for a gaming client.

#### Free

> There is a 0% protocol wide fee and runs exactly at gas cost.

Nouns Game Engine is CC0 to the maximum extent possible, which includes any bundled assets, example games, and the underlying engine code itself. There is no fee, no token gating, and no permission required to build, distribute, and sell games built with it.

#### Valuable

> Accrues value which is accessible and exitable by the owners.

As a CC0 project, there is no owner. Value is accrued in the form of direct experiences for players, and a vehicle for distribution and economic value for creators. In turn, the value creators accrue by using Nouns Game Engine to make, distribute, and sell games, should return to Nouns Game Engine in the form of contributions, additions, and extensions to core NounsGame engine, and as wholly new integrations with additional services.

#### Expansive

> There are built-in incentives for participants in the protocol.

While Nouns Game Engine does not intend to introduce any hard incentives, such as an in-game token, the ability for others to do so for their games and in-game marketplaces is an implicit incentive for participants, whether they are players obtaining incentives for playing creators' games, or creators building sustainable business models around their game content.&#x20;

For this reason, Nouns Game Engine purposefully removes itself as an arbiter of incentives, to remain agnostic and useful for all participants. NounsGame also provides expansiveness in the form of shared leaderboards and achievements, acting as an identity-based "passport" across participant creations, if the participants elect to use it.

#### Permissionless

> &#x20;**** Universally accessible and censorship resistant. Builders and users cannot be de-platformed.

NounsGame achieves this largely being an unstoppable game engine, as well as by imposing no economic model on creators. The nature of how games are created and deployed on Nouns Game Engine makes it impossible for a creator to render their game unusable by its players.&#x20;

In the event a creator ceases to evolve or expand its game offering, and that offering uses an in-game currency of their own design, and in the event that in-game currency is burned or otherwise unavailable, Nouns Game Engine could enforce a "free play" option at the protocol level, to protect abandoned games.

#### Positive sum

> It creates a win-win environment for participants to utilize the same infrastructure.

Without an owner, there is no incentive for the project to compete with other economic actors, or for economic actors to fork or otherwise divert from a shared core.

#### Credibly neutral

> The protocol is user-agnostic.

By having no owner, charging no fee, imposing no economic model, and running without the need for up-keep, Nouns Game Engine should remain user-agnostic to the greatest degree possible.

{% hint style="danger" %}
While Nouns Game Engine can guarantee its own execution on a variety of consumer machines, such as Windows, Mac, and Linux, it cannot guarantee the availability, correctness, or upkeep of games released on commercial consoles, which require their own permissions, processes, and certification. \
\
Games made with Nouns Game Engine for these appliances will still need to adhere to those restrictions, and there's no guarantee that support will exist for those platforms beyond what is shipped or provided by the engine, and the developer's own capabilities to work with those companies and hardware.\
\
A good rule thumb might be: _if it can run an Ethereum node, it can run NounsGame._
{% endhint %}

## Approach & Caveats

Nouns Game Engine is a game engine. To succeed as a hyperstructure, it must be [unstoppable](../docs/README%20\(1\).md). Game engines are inherently "living" codebases, because of the shifting capabilities of user runtime environments, graphics processing standards and devices, and the state of library and hardware support across a wide variety of concerns.

While the primary quality of unstoppability is immutable code execution, for gaming concerns it must also include determinism. Determinism in this case means predictable output from game subsystems, such as graphics rendering and physics simulation, regardless of platform.

One aspect of determinism is fixed-point math, which removes some vectors for instability where different machine architectures will de-synchronize over time. De-synchronization is a major concern for game networking, as multiple players in a game rely on a shared state to predict where other players and game objects are in a game simulation and where they should end up based on physical forces at play.

EVM does not yet support floating-point math, and so this constraint for determinism is achieved by default, though it takes more effort to properly develop and test math libraries in fixed-point.

To achieve determinism and safe-guard unstoppability, Nouns Game Engine must use Ethereum Virtual Machine (EVM) native code execution in as many sub-systems as possible, and particularly in the graphics and physics realms.

This is difficult due to the following constraints:

* The EVM is a stack machine, which is challenging to parallelize, as operations rely on shared memory whose manipulations must come in a specific order, as the values pulled from the stack for operations are implicit. Executing code in parallel is a key component of performance optimization, but is also important for coordinating the work of multiple gaming sub-systems, such as blackboard architectures for AI, and hyper-simulation of game state for predictive networking design.
* EVM graphics, such as the [compositing libraries](https://github.com/kohiart/kohi-comopser) developed by [Kohi Art Community](https://kohi.art), is software-oriented, implemented with a scanline-based rasterizer. GPUs' in-built graphics rendering is oriented around drawing triangles. An approach using compute shaders (GPGPU) and binning algorithms is likely needed to run EVM-based graphics processing at a scale suitable for gaming and real-time artworks.

It's the opinion of the Nouns Game Engine project that it is not sufficient to run third-party graphics or physics libraries, as these are a vector for continuous change, and the primary way a game engine hyperstructure could degrade, making it no longer unstoppable, as deployed code could simply stop working over a long enough time horizon without direct intervention by a project owner or contributor.

Even in the case where EVM code can run in a graphic accelerated context, the means by which that code is instrumented to run on a GPU or other appliance, through shaders or other means, must itself be unstoppable. For this, we could choose a code-generated approach with multiple targets. Though it is unlikely to ever avoid switching out the execution engine on a long enough time scale, we can use the predictable, deterministic EVM code as the singular execution model, and limit the amount of change to processes that execute it. In this way, we will at least guarantee that _something_ can always execute the code (i.e. the blockchain client's EVM implementation) at minimum, even if it does not exhibit the performance characteristics we want. In extreme cases, pre-processing or caching mechanisms can address static assets that are used in game pipelines.

#### Further Reading

{% embed url="https://jacob.energy/hyperstructures.html" %}
The nomenclature for _hyperstructures_ is defined in this article by Jacob @ Zora
{% endembed %}
