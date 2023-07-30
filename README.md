
# BloodyMailBox- Mod Server for V Rising ( Gloomrot update )

## Requirements:

For the correct functioning of this mod you must have the following dependencies installed on your server:

- [BepInExPack V Rising ](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/) 
- [VRising.VampireCommandFramework](https://v-rising.thunderstore.io/package/deca/VampireCommandFramework/) 
- [Bloodstone](https://v-rising.thunderstore.io/package/deca/Bloodstone/) 


### Actual Features

- [x] Mailbox system to receive and send messages to other users.

### Next Features

- [ ] Submit a support ticket to admins
- [ ] UI to manage and manage the mailbox
- [ ] Creation of a public API and being able to use the mod from other mods to send messages to users and store it in their mailbox


<details>
<summary>Changelog</summary>

`0.3.0`
- First public version of the mod

</details>

## Chat Commands

| COMMAND                                          |DESCRIPTION|Example
|--------------------------------------------------|-------------------------------|-------------------------------|
| `.mailbox list`   | List of all your messages in the mailbox | `.mailbox list`
| `.mailbox send <UserNick> "<Message>"`   | Send a message to the mailbox of a server user | `.mailbox send Trodi "Hello World"`
| `.mailbox read <idMessage>`   | Read a certain message from your mailbox, the id to indicate is the number that appears between [ ] when you execute the list command | `.mailbox read 1`
| `.mailbox delete <idMessage>`   | Delete a certain message from your mailbox, the id to indicate is the number that appears between [ ] when you execute the list command | `.mailbox delete 1`
| `.mailbox deleteall`   | "Delete all messages from your mailbox | `.mailbox deleteall`

# Configuration

Once the mod installed, a configuration file will be created in the \BepInEx\config server folder where you can activate or desactivate any of the mod notifications.

**BloodyMailBox.cfg**

|SECTION|PARAM| DESCRIPTION                                                     | DEFAULT
|----------------|-------------------------------|-----------------------------------------------------------------|-----------------------------|
|Config|`ownMessages`            | Allows you to send messages to your mailbox to yourself.              | false
|Config|`maxMessages`            | "Maximum number of messages that a user can have in the mailbox. If the value is 0 they are infinite (Not recommended) | 20

If you need assistance you can ask in the discord [V Rising Mod Community](https://discord.gg/vrisingmods)


# Credits

This mod idea was a suggestion from [@SynovA.cmd](https://ideas.vrisingmods.com/posts/93/ticket-system) on our community idea tracker, please go vote and suggest your ideas: https://ideas.vrisingmods.com/

[V Rising Mod Community](https://discord.gg/vrisingmods) the best community of mods for V Rising

[@Vexor Gaming](https://discord.gg/AyyenSJH) For giving me ideas and testing the mods on your server and with your community.

[@Deca](https://github.com/decaprime) for your help and the wonderful framework [VampireCommandFramework](https://github.com/decaprime/VampireCommandFramework) and [BloodStone](https://github.com/decaprime/Bloodstone) based in [WetStone](https://github.com/molenzwiebel/Wetstone) by [@Molenzwiebel](https://github.com/molenzwiebel)

[@Adain](https://github.com/adainrivers) for encouraging me to develop a UI to be able to use the mod from the client, for the support and for its [VRising.GameData](https://github.com/adainrivers/VRising.GameData) framework
