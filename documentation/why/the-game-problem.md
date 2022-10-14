# The Game Problem

#### Games need to do _dangerous things_ on your computer to operate.&#x20;

There are limits to what an EVM native game environment can achieve for low-level operations on a user computer. For example, the EVM has no opcodes or ability to access a user's hard drive, or graphics card. In some cases, empty functions in smart contracts can serve as signals to the running engine to access local resources, but the NounsGame client must respond to changes or security concerns, and therefore must be allowed to update.

That said, Nouns Game Engine's EVM native infrastructure will not change, and is usable by Nouns Game Engine, and any other game client, for executing shared hyperstructures. In this way, extending or replacing the Nouns Game Engine client is permissionless.

