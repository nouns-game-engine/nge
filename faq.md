---
description: Frequently Asked Questions about the project, and proposal.
---

# FAQ & FUD

### FAQ

Feel free to [ask us anything](https://twitter.com/NounsGameWTF). We'll post answers here.

### FUD

Addressing reality-checks for the project.

#### [Godot ](https://godotengine.org/)is [MIT](https://opensource.org/licenses/MIT) licensed, open-source, and free, why not use that?

Godot is not chain native, and so incorporating chain-native concepts into it will be an activity in opaquely integrating features. It's difficult to learn an existing, mature ecosystem at a level that enables deep changes, especially in a tight timeline. Building a new engine that is chain native from the ground-up would take less time, because we know what it needs to do, and we are more comfortable with our development platform, language, and existing tools.

#### Ethereum and the EVM are too slow to run games in real-time!

This is true, but we're trying to change that. There is no specification in the [Ethereum Yellow Paper](https://ethereum.github.io/yellowpaper/paper.pdf) for client runtime or gas limitations. This means we can write a client tailored to faster execution for  heavily read-oriented operations. The vast majority of engine use of EVM is read-only, meaning it is "gas free" as it does not require sending transactions over the network. This means we can embed an EVM locally as well as cache resources from the network, where needed, to keep performance high. We also have a proposed process for bridging EVM bytecode to run on a GPU.

#### Without liquidity or financial incentives, the project will die after completion

The project is a public good, under a [CC0 license](https://creativecommons.org/share-your-work/public-domain/cc0/), so that there is no embedded financialization, by design. This means that other actors can build those aspects onto the project, if desired. That said, we believe that a shared, public game engine can thrive, as developers who choose to use it have a natural incentive to contribute, and users that enjoy the content have a natural incentive to promote the games made with it, further attracting developers into the ecosystem.

#### How do we know the team has the ability to ship this?

You don't, other than what we've stated about our experience shipping games and custom engines, and the work we've completed to date. And hopefully, what we're delivering over time and exhibiting here.

#### Why not just use Unity or Unreal?

One of the main purposes to creating an alternative to these major industry tools is that they are not CC0 licensed, and are not built with those principles, and are not inherently chain native.&#x20;

#### You'll never catch up to Unity/Unreal, they represent uncountable years of engineering effort!

This is accurate, though we are not trying to achieve an unrealistic outcome. What we're trying to do is build a solid foundation on which the collective efforts of many can capture effort, make it immutable and unstoppable on Ethereum, and produce a "[flywheel](https://www.jimcollins.com/concepts/the-flywheel.html)" effect that continuously delivers more value, and could potentially match or outstrip the capabilities of modern commercial engines in the future.

#### Why [Nouns DAO ](https://nouns.wtf)and not venture capital?

Because Nouns DAO has the same values, and understands public goods in a way no other organization currently does. There is no financial return hanging over the project, so that it can focus on delivering its promises and not on an exit.

#### Why Nouns DAO and not an NFT project to raise funds?

An NFT raise is a possibility, but it impedes the project's ability to freely distribute to all. An NFT raise would be tied to future utility for early holders. The utility can't be access to the engine as this is a public good. It can't be a token gate to one of the game deliverables because these are for everyone to spread the Nouns meme. Ultimately, the NFT would have disconnected utility, possibly distracting from the larger project as it would induce its own special deliverable. In the end, if the NFT is nothing more than a token of appreciating for donating, then it isn't aligned with its collectors, which is why we haven't considered this route for now.

#### Why not just build a game? Making an engine versus a game is backwards!

It might be possible to build a game, and that would be the default plan if no funding is raised, as it opens up a more focused delivery and the potential to fundraise through an NFT. That said, an engine is a public good, while a game by definition is limited in that it isn't a means of producing many games.

#### Why is there no token or governance for the project?

Our opinion is that governance makes sense for things that can be reduced to concise, verifiable, on-chain actions, like sending funds, or setting auction parameters, or updating contract addresses. And in some limited cases, perhaps, it is useful for ranking upcoming work in order of preference. Where it isn't effective is in dictating features or design. As a public good, licensed as CC0, there is no need for a governance model, though these can be built on top, or the project can fork in a different direction based on the needs and desires of another community.&#x20;
