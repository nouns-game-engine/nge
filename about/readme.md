# Introduction

### Vision

NounsGame is a CC0 general-purpose game engine. It has two primary reasons to exist:

* Be a public good for game development in the form of a hyperstructure
* Be an infinite source of Nouns meme proliferation through composable, distributable, cross-platform Nounish game experiences

### What is a Hyperstructure?

A term coined by [Jacob @ Zora](https://twitter.com/js\_horne), a hyperstructure is a [description of a software system](https://jacob.energy/hyperstructures.html) with specific properties. in NounsGame terms, these properties are:

#### Unstoppable

> The protocol cannot be stopped by anyone. It runs for as long as the underlying blockchain exists.

For NounsGame, this is the most important aspect of a hyperstructure. A game engine is a collection of ever-evolving parts. To achieve unstoppability, it must run on the native execution layer of its host blockchain, in this case Ethereum's EVM.&#x20;

This is achievable through a gradient of processes, including deterministic graphics rasterization and physics simulation, storage and encoding of assets, runtime code generation of client-side backends, and component interaction with the blockchain network.

#### Free

> There is a 0% protocol wide fee and runs exactly at gas cost.

NounsGame is CC0 to the maximum extent possible, which includes any bundled assets, example games, and the underlying engine code itself. There is no fee, no token gating, and no permission required to build, distribute, and sell games built with it.

#### Valuable

> Accrues value which is accessible and exitable by the owners.

As a CC0 project, there is no owner. Value is accrued in the form of direct experiences for players, and a vehicle for distribution and economic value for creators. In turn, the value creators accrue by using NounsGame to make, distribute, and sell games, should return to NounsGame in the form of contributions, additions, and extensions to core NounsGame engine, and as wholly new integrations with additional services.

#### Expansive

> There are built-in incentives for participants in the protocol.

While NounsGame does not intend to introduce any hard incentives, such as an in-game token, the ability for others to do so for their games and in-game marketplaces is an implicit incentive for participants, whether they are players obtaining incentives for playing creators' games, or creators building sustainable business models around their game content.&#x20;

For this reason, NounsGame purposefully removes itself as an arbiter of incentives, to remain agnostic and useful for all participants. NounsGame also provides expansiveness in the form of shared leaderboards and achievements, acting as an identity-based "passport" across participant creations, if the participants elect to use it.

#### Permissionless

> &#x20;**** Universally accessible and censorship resistant. Builders and users cannot be de-platformed.

NounsGame achieves this largely being an unstoppable game engine, as well as by imposing no economic model on creators. The nature of how games are created and deployed on NounsGame makes it impossible for a creator to render their game unusable by its players.&#x20;

In the event a creator ceases to evolve or expand its game offering, and that offering uses an in-game currency of their own design, and in the event that in-game currency is burned or otherwise unavailable, NounsGame could enforce a "free play" option at the protocol level, to protect abandoned games.

#### Positive sum

> It creates a win-win environment for participants to utilize the same infrastructure.

Without an owner, there is no incentive for the project to compete with other economic actors, or for economic actors to fork or otherwise divert from a shared core.

#### Credibly neutral

> The protocol is user-agnostic.

By having no owner, charging no fee, imposing no economic model, and running without the need for up-keep, NounsGame should remain user-agnostic to the greatest degree possible.

{% hint style="danger" %}
While NounsGame can guarantee its own execution on a variety of consumer machines, such as Windows, Mac, and Linux, it cannot guarantee the availability, correctness, or upkeep of games released on commercial consoles, which require their own permissions, processes, and certification. \
\
Games made with NounsGame for these appliances will still need to adhere to those restrictions, and there's no guarantee that support will exist for those platforms beyond what is shipped or provided by the engine, and the developer's own capabilities to work with those companies and hardware.\
\
A good rule thumb might be: _if it can run an Ethereum node, it can run NounsGame._
{% endhint %}
