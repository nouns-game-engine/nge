---
description: The scope of work for this proposal.
---

# Scope

{% hint style="danger" %}
This document is a very rough draft and will change frequently based on feedback and design sessions.
{% endhint %}

## Term

This proposal is expected to run for two years and funding is scoped for that timeframe, in one year iterations. During the project funding window, we expect to deliver the key deliverables, and potentially additional deliverables based on status and any challenges or setbacks encountered.

### Component Deliverables

Success of the project depends on known component deliverables, the absence of which blocks use of the engine as a public good. The list of component deliverables is broken down in [year one](year-one.md) and [year two](year-two.md).

### Additional Deliverables

The project is greatly enhanced with these deliverables, but they are not required for successful operation of the hyperstructure. These could be added to the scope based on the progress of the main delivery, and are low to moderate risk.

* **Targeted GPU-Accelerated EVM rendering**
  * Local EVM instance w/ render function detection
  * Offloading of EVM bytecode to runtime process for GPU preparation/caching
* **Engine Modes**
  * [Physically-Based Rendering](https://en.wikipedia.org/wiki/Physically\_based\_rendering) (PBR) 3D world w/ 3D physics
* **Integrated Network**
  * P2P "Couch Co-Op" Example
  * [Steam](https://store.steampowered.com/) Integration w/ Examples
* **Generative Simulation Testing**
* **Achievements Subsystem**
* **Leaderboard Subsystem**
* ****[**NAT Punch-Through**](https://en.wikipedia.org/wiki/Hole\_punching\_\(networking\)) **Helpers**

### Out Of Scope

These deliverables are part of the project plan, but currently omitted from the scope of the project as they represent high-risk, high probability of failure within the timeline, but listed here for completeness and/or they present compelling, potential future work.&#x20;

* **General Purpose Networking**
* **General Purpose EVM/GPU Acceleration**

### A Note On Networking

{% hint style="danger" %}
Production game-quality networking is a difficult problem in games, and there is no "one size fits all" solution.&#x20;
{% endhint %}

Some games work with turn-based, occasionally connected devices, which is achievable at the smart contract level with Ethereum's existing infrastructure, and forms the basis of most on-chain games today.&#x20;

> _**N.B.**_**  **_**This form of networking requires no additional work for the engine outside of the existing "Game State Serializer" deliverable with "State Machine Rollup" to store saved game states on-chain in an efficient way.**_

Some need peer-to-peer networking (couch co-op).&#x20;

Some need large server farms (i.e. [MMORPGs](https://en.wikipedia.org/wiki/Massively\_multiplayer\_online\_role-playing\_game)).

Some need medium throughput, packet-based, [protocol](https://wiki.vg/Protocol) level networking (i.e. [open world/sandbox games](https://en.wikipedia.org/wiki/Open\_world)).

Some need [rollback](https://en.wikipedia.org/wiki/GGPO) models (i.e. [fighting games](https://en.wikipedia.org/wiki/Fighting\_game)).\
\
Some games need [lock-step](https://en.wikipedia.org/wiki/Lockstep\_protocol) models (i.e. [RTS ](https://en.wikipedia.org/wiki/Real-time\_strategy)games).

### So what will NounsGame provide for networking?

What we can do, is help ensure games built with Nouns Game Engine are network-safe by default. We can do this by providing all the necessary integration points and support tooling for [client-side prediction and reconciliation](https://en.wikipedia.org/wiki/Client-side\_prediction), use network-stable serialization primitives by default, and provide common interfaces and tools for desynchronization comparisons, latency simulation, and other [netcode ](https://en.wikipedia.org/wiki/Netcode)concerns.&#x20;

In other words, using Nouns Game Engine, it should be possible to avoid "_shooting yourself in the foot_", as the engine design and tools will work with you, sometimes with tough love, to ensure your game has a higher success rate when adding networking.

{% hint style="info" %}
It's possible that _some_ networking examples can be bundled with the example games, but it is out-of-scope to avoid all of the above, and to not over-subscribe a networking architecture that is impossible to deliver in the timeline, and with the [quality and security](https://www.reddit.com/r/Unity3D/comments/6wjxu7/why\_does\_everyone\_hate\_on\_unet\_is\_it\_really\_that/) that would make it a useful public good.
{% endhint %}
