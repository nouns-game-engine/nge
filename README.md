# nounsgame.wtf

A [Nouns](https://nouns.wtf)-based CC0 game and engine.

![Screenshot](/docs/screenshot.png)

Read the [prop.house Proposal](https://prop.house/proposal/534)

# Description
## What are we building?

While incorporating Nouns into existing decentralized gaming properties is wise, this project aims to align game development tooling to the Nouns spirit, by releasing all work, to the greatest extent possible, as CC0.

This means anyone can make, mix, and re-master their own games from the codebase, and proliferate Nouns across gaming devices.

[nounsgame.wtf](https://nounsgame.wtf) is a project to achieve this vision. The game engine will possess the following properties:

- CC0 of all game engine code, without dependencies on any copyleft or proprietary libraries or SDKs (Unreal, Unity, et. al)
-  Support for open asset formats
- Cross-platform across Windows, Mac, and Linux by default, with well-known and battle-tested on-ramps to console SDKs. We also now have an experimental web-based build using WebAssembly
- Designed such that the running game is also the development tooling. This is important, as many game projects ship bespoke, platform-specific editors and pipelines that make it harder to main and distribute
- Designed with the intent to minimize the knowledge required for non-technical creators to contribute, fork, or expand the game or create their own game
- The ability to run EVM code as intrinsic to the game state.

## Roadmap

This is an ambitious project. Game projects require an immense amount of work and the coordination of many to realize. This proposal will not encapsulate the entire work, but will go a long way towards establishing the baseline design by providing the development time needed to produce the game runtime environment, asset management pipeline, and necessary architecture to accept contributions from the outside as well as create a "sandbox" for further proposals to build from.

## Who is building this?

[Wattsy](https://twitter.com/wattsyart), lead developer of [Kohi](https://kohi.art), an on-chain generative art platform for Ethereum, is leading the development of this proposal and set of milestones. He is also active in the [CrypToadz](https://cryptoadz.io) community, currently working to bring the [CrypToadz on-chain](https://github.com/wattsyart/cryptoadz-chained).

## Compatibility

| Platform | Status       |
|----------|--------------|
| Windows  | stable       |
| Mac      | stable       |
| Linux    | stable       |
| Web      | experimental |

## Socials

[@nounsgamewtf](https://twitter/nounsgamewtf)
