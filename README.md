# RexBot-2.0
Rewritten Rexbot with Discord.NET 1.0.0-rc2 with more features than ever!

```diff
- This bot is discontinued as of 5/20/2017 as it was posting too many memes for some people to handle. 
- Use with caution.
```

This bot isn't meant to be run by anyone other than me or my friends so I will not detail how to run this bot. Although, it's as simple as double clicking a certain program.

If you are really interested in running this bot for yourself, you will need your own **discord bot token, twitter consumer/user access tokens and keys, imgur client id, xmashtape key, youtube api key, and microsoft supscription key**.
Obtaining all of those will take you around 3 hours if you haven't created dev accounts for each one. Fun stuff.

### Features
- 53 Commands divided into 10 categories
- Search youtube, urban dictionary, imgur, twitter, giphy and much more
- Post random cat, dog photos
- Full translate support using Bing translator
- Meme creator with easy syntax
- Funny sentence generator + customizable vocabulary
- Alias system which lets the bot understand who you are referring to
- See guild stats such as the number of commands invoked, messages received, emotes used
- Admin functions: turn off the bot via command, delete messages in bulk, change the mode to control bot activation frequency, restrain users
- Join a voice chat room and play music
- Silly troll functions to annoy your guildmates (togglable with mode / restraints) (Most troll functions have a per user cooldown)
- Ability to send PM's and cross channel talk support
- React to certain words with given strings
- Detailed Logs to traceback errors
- And much more!

### Notes
Adding user to dictionary requires adding to alias2.txt **AND** adding to usernameDict in DataUtils.cs.

Alias Dictionary is loaded from txt file while usernameDict is populated internally.

Bing authentication is done automatically when given client-id key.

WebHook gets you guild,channel id if needed.

And yes, it is better to use a proper db rather than load and save to txt files. 
All of this was put tgt in less than a week and I just wanted to play with different api's.
Current code is easily modifiable to accomodate MongoDB. Just change the populate methods in DataUtils.

Services were not properly utilized as I was noob to discord when I started using this bot.
Utils folder can be organized into services. Can create more handler classes to deal with different events as well.

