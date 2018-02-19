using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using RexBot2.Utils;
using System.Linq;
using RexBot2.Timers;
using Discord;
using Discord.WebSocket;
using RexBot2.Objects;
using System;

namespace RexBot2.Modules
{
    public class TrollModule : ModuleBase<SocketCommandContext>
    {
        private string picPath = "Data/pics/";
        [Command("eminem")]
        [Remarks("troll")]
        [Summary("Rap God")]
        public async Task eminemCmd()
        {
            string username = Context.User.ToString();
            if (RexTimers.canRunCmd(username, "eminem"))
            {
                await Context.Channel.SendFileAsync(picPath + "eminem.jpg");
                await Context.Channel.SendMessageAsync("PALMS SPAGHETTI KNEAS WEAK ARM SPAGHETTI THERES SPAGHETTI ON HIS SPAGHETTI ALREADY, MOMS SPAGHETTI", true);
                RexTimers.resetTimer(username, "eminem");
            } else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "eminem") + "`");
            }
        }

        [Command("w")]
        [Remarks("troll")]
        [Summary("A chance to be annoying")]
        public async Task wCmd()
        {
            string username = Context.User.ToString();
            if (RexTimers.canRunCmd(username, "w"))
            {
                int addchances = 0;
                if (DataUtils.waddchances.ContainsKey(username))
                {
                    addchances = DataUtils.waddchances[username];
                }
                bool www = MasterUtils.roll(17+addchances);
                string res = "`" + MasterUtils.stripName(username) + " had a (17+" + addchances + ") = " + (17+addchances)+"% chance, but failed miserably`";
                res += "\n`No W's for you today " + MasterUtils.stripName(Context.User.ToString()) + "!`";
                if (!www)
                {
                    await Context.Channel.SendMessageAsync(res);
                }
                else
                {                    
                    await Context.Channel.SendMessageAsync(MasterUtils.getAnnoyingTTSString(), true);
                }
                RexTimers.resetTimer(username, "w");
            } else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "w")+ "`");
            }
        }

        [Command("forgive")]
        [Remarks("troll")]
        [Summary("forgive a fool")]
        public async Task forgiveCmd([Remainder] string name)
        {
            string username = Context.User.ToString();

            if (RexTimers.canRunCmd(username, "forgive") || MasterUtils.ContainsAny(username, GlobalVars.ADMINS))
            {
                if (AliasUtils.getAliasKey(name).Contains("None"))
                {
                    await Context.Channel.SendMessageAsync("You're trying to forgive an unregistered user!");
                }
                else
                {
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];

                    if (name == username)
                    {
                        await Context.Channel.SendMessageAsync("Are you seriously trying to forgive yourself bruh?");
                        RexTimers.resetTimer(username, "forgive");
                        return;
                    }

                    if (DataUtils.reports.ContainsKey(name))
                    {
                        if (DataUtils.reports[name] <= 0)
                        {
                            await Context.Channel.SendMessageAsync("You can't forgive someone with 0 or less reports");
                        } else
                        {
                            DataUtils.gainReports(name, -1);
                            await Context.Channel.SendMessageAsync("You successfully forgave this fool");
                        }                        
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("This angel has not been reported yet");
                    }
                    
                    RexTimers.resetTimer(username, "forgive");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "forgive") + "`");
            }
        }

        [Command("report")]
        [Remarks("troll")]
        [Summary("report a fool")]
        public async Task reportCmd([Remainder] string name)
        {
            string username = Context.User.ToString();
            if (RexTimers.canRunCmd(username, "report") || MasterUtils.ContainsAny(username, GlobalVars.ADMINS))
            {
                if (AliasUtils.getAliasKey(name).Contains("None"))
                {
                    await Context.Channel.SendMessageAsync("You're trying to report an unregistered user!");
                }
                else
                {
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];

                    DataUtils.incReports(name);

                    await Context.Channel.SendMessageAsync("Report successful");
                    RexTimers.resetTimer(username, "report");
                }
            } else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "report") + "`");
            }
        }

        [Command("reports")]
        [Remarks("troll")]
        [Summary("show all reports")]
        public async Task reportsCmd()
        {
            string res = string.Empty;
            foreach (KeyValuePair<string, int> kv in DataUtils.reports)
            {
                res += "User " + kv.Key + ", reported " + kv.Value + " times!\n";
            }
            if (res == string.Empty)
            {
                res += "Nobody has been reported! YET...";
            }

            await Context.Channel.SendMessageAsync(res);
        }

        [Command("emoji")]
        [Remarks("troll")]
        [Summary("React to the last message with a random Emoji")]
        public async Task emoteCmd()
        {            
            var messages = await Context.Channel.GetMessagesAsync((1)).Flatten();
            foreach(SocketUserMessage msg in messages)
            {
                //int count = DataUtils.rnd.Next(1, 4);
                //for(int i=0; i < count; i++)
                //{
                await msg.AddReactionAsync(EmojiUtils.getRandEmoji());
                //}                
            }
        }

        [Command("fuckyourexbot")]
        [Remarks("troll")]
        [Summary("Something Nick says often")]
        public async Task fyrbCmd()
        {
            double duration = DataUtils.rnd.Next(20, 40);
            AdminUtils.addRestriction(Context.User.ToString(), duration);
            await Context.Channel.SendMessageAsync("No, fuck you " + Context.User.Mention + "\nIma restrain you for " + duration + "s\n");   
        }

        [Command("iloveyourexbot")]
        [Remarks("troll")]
        [Summary("Something Nick doesnt say often")]
        public async Task iluCmd()
        {
            double duration = DataUtils.rnd.Next(20, 40);
            AdminUtils.addRestriction(Context.User.ToString(), duration);
            await Context.Channel.SendMessageAsync("Well, I dont! lol " + Context.User.Mention + "\nIma restrain you for " + duration + "s\n");
        }

        [Command("invest")]
        [Remarks("troll")]
        [Summary("Are you a business man?")]
        public async Task investCmd()
        {

            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(177, 44, 33);
            emb.Title = "** Company name / Risk / Max Reward / Ethicality / Share Cost / Duration **\n";
            string desc = "";
            foreach (Company comp in DataUtils.companies)
            {
                desc += comp.ToString() + "\n";
            }
            emb.Description = desc;
            await Context.Channel.SendMessageAsync("", false, emb);
        }

        [Command("invest")]
        [Remarks("troll")]
        [Summary("Are you a business man?")]
        public async Task investzCmd(int companyindex)
        {
            string username = Context.User.ToString();
            

            if (!DataUtils.existsVisibleCompanyWIthID(companyindex))
            {
                await Context.Channel.SendMessageAsync("Invalid Company ID. Pick from the list of companies you see in !invest");
                return;
            }

            Company comp = DataUtils.getCompanyWithID(companyindex);

            if (DataUtils.alreadyInvestedInCompany(username, DataUtils.getCompanyWithID(companyindex)))
            {
                await Context.Channel.SendMessageAsync("You can't invest in the same company more than once!");
                return;
            }

            if(DataUtils.getCompanyWithID(companyindex).AvailableShare > DataUtils.coins[username])
            {
                await Context.Channel.SendMessageAsync("You dont have enough coins to invest in this company.");
                return;
            }

            int cost = DataUtils.getCompanyWithID(companyindex).AvailableShare;
            DataUtils.gainCoins(username, -cost);
            DataUtils.makeInvestment(username, DataUtils.getCompanyWithID(companyindex));

            await Context.Channel.SendMessageAsync(username + " successfully invested " + cost +" coins in " + comp.Name + "!\nCheck back in "+ comp.getDurationString() + " to see how successful your investment was!");
        }

        [Command("checkinvestments")]
        [Alias("ci","myinvestments","checkinvestment")]
        [Remarks("troll")]
        [Summary("Are you a business man?")]
        public async Task checkinvestCmd()
        {
            string username = Context.User.ToString();

            if (!DataUtils.hasMadeAnInvestment(username))
            {
                await Context.Channel.SendMessageAsync("You haven't made any investments recently!");
                return;
            }

            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(177, 44, 33);
            emb.Title = "** Company name / Coins Invested / Risk / Time left **\n";
            string desc = "";
            foreach (KeyValuePair<Company, DateTime> kvp in DataUtils.investments[username])
            {
                Company comp = kvp.Key;
                TimeSpan ts = kvp.Value -DateTime.Now;
                if (ts.TotalSeconds < 0)
                {
                    //int risksub = DataUtils.rnd.Next(0, comp.Risk);
                    int percentageGain = DataUtils.rnd.Next(90 - comp.Risk * 35, comp.MaxReward);
                    int profit = comp.AvailableShare * (percentageGain)/ 100;
                    await Context.Channel.SendMessageAsync("Your investment of `" + comp.AvailableShare + "` coins in `" + comp.Name +
                        "` resulted in `" + profit.ToString() +  "` coins. This company had a risk value: `" + comp.getRiskString()
                        + "` and the max possible reward was `" + comp.MaxReward + "%`. The actual reward was `" + percentageGain + "%`. "
                        + "I would rate your investment as: `" + DataUtils.getInvestmentSuccessString(comp.MaxReward, percentageGain) + "`\n The ethicality rating for this company was `" +
                        comp.getEthicalityString() + "` and Rexbot's attitude towards you will change accordingly");
                    DataUtils.gainCoins(username, profit);
                    DataUtils.investments[username].Remove(comp);
                } else
                {
                    desc += comp.Name + " / " + comp.AvailableShare + " / " + comp.getRiskString() + " / " + ts.ToString("h'h 'm'm 's's'") + "\n";
                }
                
            }

            emb.Description = desc;
            await Context.Channel.SendMessageAsync("", false, emb);
        }

        [Command("exchangerate")]
        [Alias("exchange","er")]
        [Remarks("troll")]
        [Summary("Check the current exchangerate")]
        public async Task erCmd()
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(0, 255, 255);
            emb.Title = "** Exchange Rate **\n";
            string desc = "";
            desc += "[reports<100] -> 0\n";
            desc += "[reports<300] -> 50%\n";
            desc += "[reports<500] -> 70%\n";
            desc += "[reports<700] -> 85%\n";
            desc += "[reports<900] -> 100%\n";
            desc += "[reports<1001] -> 150%\n";
            desc += "[reports>1000] -> 100%\n\n";
            desc += "Use !convert to convert your reports to coins!";

            emb.Description = desc;
            await Context.Channel.SendMessageAsync("", false, emb);
        }

        [Command("convert")]
        [Remarks("troll")]
        [Summary("Exchange reports into rex coins according to exchange rate")]
        public async Task convCmd()
        {
            string username = Context.User.ToString();

            int reportCount = DataUtils.getReportCount(username);
            int coinsToGain = 0;

            if (reportCount < 100)
            {
                coinsToGain = 0;
                
            } else if(reportCount < 300)
            {
                coinsToGain = reportCount / 2;
            }
            else if (reportCount < 500)
            {
                coinsToGain = (int)Math.Round(reportCount * 0.7);
            }
            else if (reportCount < 700)
            {
                coinsToGain = (int)Math.Round(reportCount * 0.85);
            }
            else if (reportCount < 900)
            {
                coinsToGain = reportCount;
            }
            else if (reportCount < 1001)
            {
                coinsToGain = (int)Math.Round(reportCount * 1.5);
            } else
            {
                coinsToGain = reportCount;
            }
            //reset reports
            DataUtils.setReports(username, 0);
            DataUtils.gainCoins(username, coinsToGain);

            await Context.Channel.SendMessageAsync("`" + username + " exchanged " + reportCount + " reports for " + coinsToGain + " coins!`");
        }


        [Command("buy")]
        [Remarks("troll")]
        [Summary("Buy item from shop. Syntax : Buy <itemname>")]
        public async Task buyCmd([Remainder]string query)
        {
            
            string username = Context.User.ToString();
            //check Cooldown
            if (!RexTimers.canRunCmd(username, "buy"))
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "buy") + "`");
                return;
            }


               // DataUtils.coins[username] = 1000; //test purposes
            //Check query argument count validity
            string[] words = query.Split(' ');

            //this condition will never be met.. as fun;ction will not be called (will call !help !buy)
            if(words.Length < 1)
            {
                await Context.Channel.SendMessageAsync("Specify what you want to buy");
                return;
            }

            int index = DataUtils.shop.FindIndex(f => f.Callname == words[0]);

            if (index < 0)
            {
                await Context.Channel.SendMessageAsync("You entered an invalid item");
                return;
            }

            ShopItem item = DataUtils.shop.ElementAt(index);

            if(item.Argcount != words.Length)
            {
                await Context.Channel.SendMessageAsync("Check your arguments bruh");
                return;
            }

            //Check if we have enough coins to buy item
            int cost = item.Cost;

            if (!DataUtils.canBuy(username, cost))
            {
                await Context.Channel.SendMessageAsync("You dont have enough money you poor thing");
                return;
            }

            //Actually buy the item
            DataUtils.spendCoins(username, cost);
            string name;
            int dice;


            switch (item.Callname)
            {
                case "report":
                    //check if valid user -> 
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user!");
                    }                        
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    dice = DataUtils.rnd.Next(1, 101);

                    if(dice < 50)
                    {
                        DataUtils.gainReports(name, 77);
                        await Context.Channel.SendMessageAsync(name + "has been reported 77 times LOL!");
                    } else
                    {
                        await Context.Channel.SendMessageAsync("I decided not to report anyone");
                    }                    
                    break;
                case "forgive":
                    //check if valid user -> 
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user!");
                    }
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    dice = DataUtils.rnd.Next(1, 101);
                    if (dice < 50)
                    {
                        DataUtils.gainReports(name, -77);
                        await Context.Channel.SendMessageAsync(name + "has been forgiven 77 times!");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("I decided not to forgive anyone");
                    }
                    break;
                case "w":
                    await Context.Channel.SendMessageAsync(MasterUtils.getAnnoyingTTSString(), true);
                    break;
                case "wchance":
                    DataUtils.incWAddChances(username);
                    await Context.Channel.SendMessageAsync("Successfully increased your !w chances by 1%");
                    break;
                //case "beg":
                    //break;
                case "catmode":
                    //length of temp catmode in MasterHandler
                    RexTimers.catModeClock.Start();
                    break;
                case "restrain":
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user!");
                    }
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];

                    int timeInSeconds = DataUtils.rnd.Next(121, 181);
                    AdminUtils.addRestriction(name, timeInSeconds);
                    await Context.Channel.SendMessageAsync(name + " is restrained for " + timeInSeconds + "s!");
                    break;
                case "purge":
                    if(int.Parse(words[1]) > 30)
                    {
                        await Context.Channel.SendMessageAsync("You tried to purge too many messages! What a waste of coins...");
                    }
                    var messages = await Context.Channel.GetMessagesAsync((int.Parse(words[1]) + 1)).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                    break;
                case "tts":
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user! What a waste of coins...");
                    }
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    RexTimers.addPersonToTTS(name);
                    await Context.Channel.SendMessageAsync("```" + username + " decided to tts-annoy " + name +"!\n" + name + " better start sending messages for the next 3 minutes or i'm going to take all of your coins!```");
                    break;
                case "annoy":
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user! What a waste of coins...");
                    }
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    RexTimers.addPersonToAnnoy(name);
                    await Context.Channel.SendMessageAsync("```" + username + " decided to super annoy " + name + "!\n" + name + " better start sending messages for the next 3 minutes or i'm going to take all of your coins!```");
                    break;
                case "confuse":
                    name = words[1];
                    if (AliasUtils.getAliasKey(name).Contains("None"))
                    {
                        await Context.Channel.SendMessageAsync("Invalid user! What a waste of coins...");
                    }
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    RexTimers.addPersonToConfuse(name);
                    await Context.Channel.SendMessageAsync("```"+ username + " decided to confuse " + name + "!\n" + name + " better start sending messages for the next 3 minutes or i'm going to take all of your coins!```");
                    break;
                case "bribe":
                    await Context.Channel.SendMessageAsync("This function is not implemented yet! What a waste of coins...");
                    break;

                default: await Context.Channel.SendMessageAsync("Item not added to switch statement"); break;
            }

            //show coins spent and coins remaining
            await Context.Channel.SendMessageAsync("`" + username + " paid " + cost + "coins and has " + DataUtils.getCoinCount(username) + " left!`");
            RexTimers.resetTimer(username, "buy");
            //Check if valid argument types..? -> Just force convert..
            //await Context.Channel.SendMessageAsync("Your requested item is at index :" + index + " which costs " + DataUtils.shop.ElementAt(index).Cost);
        }

        [Command("sendemail")]
        [Alias("email")]
        [Remarks("troll")]
        [Summary("Send an email to a user : sendemail <username> <contents>")]
        public async Task emailCmd(string recipient, [Remainder]string emailcontents)
        {
            string sentBy = Context.User.ToString();
            //check Cooldown
            if (!RexTimers.canRunCmd(sentBy, "email"))
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(sentBy, "email") + "`");
                return;
            }
            
            if (AliasUtils.getAliasKey(recipient).Contains("None"))
            {
                await Context.Channel.SendMessageAsync("Invalid user!");
            }
            recipient = DataUtils.aliases[AliasUtils.getAliasKey(recipient)];

            DataUtils.sendEmail(sentBy, recipient, emailcontents);

            var messages = await Context.Channel.GetMessagesAsync(((int)1)).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            await Context.Channel.SendMessageAsync("`" + sentBy + " successfully sent an email to " + recipient+"`");
            RexTimers.resetTimer(sentBy, "email");
        }

        [Command("checkinbox")]
        [Alias("inbox","checkemail","checkemails")]
        [Remarks("troll")]
        [Summary("Shows the last email you recieved")]
        public async Task inboxCmd()
        {
            string username = Context.User.ToString();
            if (!DataUtils.hasEmail(username))
            {
                await Context.Channel.SendMessageAsync("You have no unread emails");
                return;
            }
            await Context.Channel.SendMessageAsync("You have " + DataUtils.getEmailCount(username) + " unread emails!\nHere is your most recent one:\n\n" + DataUtils.getLastEmail(username),true);
        }

        [Command("gift")]
        [Remarks("troll")]
        [Summary("Get your daily gift here!")]
        public async Task giftCmd()
        {
            string username = Context.User.ToString();
            //check Cooldown
            if (!RexTimers.canRunCmd(username, "gift"))
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "gift") + "`");
                return;
            }

            int gift = DataUtils.rnd.Next(0, 122);
            DataUtils.gainReports(username, gift);

            await Context.Channel.SendMessageAsync("Your gift: " + gift + " reports!\n"+"Come back to me after 10000 seconds");

            RexTimers.resetTimer(username, "gift");
        }

        [Command("pickup")]
        [Remarks("troll")]
        [Summary("Hey are those coins on the ground?")]
        public async Task pickupCmd()
        {
            string username = Context.User.ToString();

            if(DataUtils.coinsOnGround <= 0)
            {
                DataUtils.gainCoins(username, -1);
                await Context.Channel.SendMessageAsync("There were no coins on the ground and " + username +" got pickpocketed while s/he was leaning down. (-1 coin)");
                return;
            }

            await Context.Channel.SendMessageAsync(username + " just picked up " + DataUtils.coinsOnGround + " coins from the ground!");
            DataUtils.gainCoins(username, DataUtils.coinsOnGround);
            DataUtils.coinsOnGround = 0;
        }

        [Command("shop")]
        [Remarks("troll")]
        [Summary("Show shop items")]
        public async Task shopCmd()
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(0, 255, 0);
            //emb.ThumbnailUrl = "http://pngimages.net/sites/default/files/bar-chart-png-image-892.png";            

            emb.Title = "**👜 Shop 👜**\n";
            //emb.Url = "https://www.youtube.com/watch?v=4YpTLy6dn5c";
            emb.Description = "\nWelcome To the Rexbot Shop!\n\n";


            //can i loop this?

            foreach(ShopItem si in DataUtils.shop)
            {                
                EmbedFieldBuilder superReportsField = new EmbedFieldBuilder();
                superReportsField.Name = si.Name;
                superReportsField.Value = "*Cost:* " + si.Cost + "\n*Syntax:* " + si.Syntax + "\n*Effect:* " + si.Description;
                superReportsField.IsInline = true;

                emb.AddField(superReportsField);
            }

            string picPath = "Data/pics/";
            

            await Context.Channel.SendMessageAsync("", false, emb);
            await Context.Channel.SendFileAsync(picPath + "welcometoshop2.png");
        }

        [Command("newbeg")]
        [Alias("nbeg","nb")]
        [Remarks("troll")]
        [Summary("Has a chance to forgive you or give you a worse punishment")]
        public async Task begCmd()
        {
            string username = Context.User.ToString();

            if (!DataUtils.reports.ContainsKey(username))
            {
                await Context.Channel.SendMessageAsync("Not reported yet");
                return;
            }

            if (RexTimers.canRunCmd(username, "beg"))//|| MasterUtils.ContainsAny(username, GlobalVars.ADMINS)
            {                
                double dice = DataUtils.rnd.Next(1, 101);
                string punishment = "";
                int oldReportCount = DataUtils.reports[username];
                int newReportCount = oldReportCount;


                if (dice < 21)
                {
                    newReportCount = oldReportCount + 7;
                    punishment = "You don't deserve anything good";
                } else if(dice < 41)
                {
                    newReportCount = oldReportCount - 5;
                    punishment = "I'm feeling generous";
                } else if(dice < 51)
                {
                    newReportCount = oldReportCount + 13;
                    punishment = "Haha middle finger up your bum";
                } else if(dice < 61)
                {
                    newReportCount = oldReportCount - 10;
                    punishment = "You better start to like me";
                } else if(dice < 71)
                {
                    newReportCount = oldReportCount + 25;
                    punishment = "gfi idiot";
                } else if(dice < 81)
                {
                    newReportCount = oldReportCount - 20;
                    punishment = "you lucky bastard";
                } else if(dice < 86)
                {
                    newReportCount = oldReportCount + 42;
                    punishment = "hahaha u failed so hard";
                } else if(dice < 91)
                {
                    newReportCount = oldReportCount - 35;
                    punishment = "That's a lot of reports gone you lucky mf";
                } else if(dice < 94)
                {
                    newReportCount = oldReportCount +150;
                    punishment = "HAHA in yo face idiot";
                } else if(dice < 97)
                {
                    newReportCount = oldReportCount -150;
                    punishment = "its your lucky day m8";
                } else if(dice < 99)
                {
                    newReportCount = oldReportCount +320;
                    punishment = "Woops";
                } else if (dice == 99)
                {
                    newReportCount = 0;
                    punishment = "You're a zero not a hero";
                } else
                {
                    newReportCount = oldReportCount + 500;
                    punishment = "YOU JUST HIT THE REPORT JACKPOT";
                }

                if (newReportCount > GlobalVars.MAX_REPORTS)
                {
                    newReportCount = 0;
                    DataUtils.gainCoins(username, GlobalVars.MAX_REPORTS);
                    await Context.Channel.SendMessageAsync("Gratz! You went above the report cap. Auto exchanged reports to coins!", true);
                    punishment += "\nGratz! You went above the report cap. Auto exchanged reports to coins!";
                }
                if (newReportCount < 0)
                {
                    newReportCount = 0;
                    punishment += "\nBtw your report count went below 0 so I set it to 0";
                }

                punishment += "\n\n**Report count : " + oldReportCount + " -> " + newReportCount+"**";
                DataUtils.setReports(username, newReportCount);


                EmbedBuilder emb = new EmbedBuilder();
                emb.Color = new Color(255, 0, 255);

                emb.Title = "**New Beg Info**\n";
                string desc = "1-20 : +7 reports\n";
                desc += "21-40 : -5 reports\n";
                desc += "41-50 : +13 reports\n";
                desc += "51-60 : -10 reports\n";
                desc += "61-70 : +25 reports\n";
                desc += "71-80 : -20 reports\n";
                desc += "81-85 : +42 reports\n";
                desc += "86-90 : -35 reports\n";
                desc += "91-93 : +150 reports\n";
                desc += "94-96 : -150 reports\n";
                desc += "97-98 : +320 reports\n";
                desc += "99 : set report count to 0\n";
                desc += "100 : +500 reports\n\n";
                desc += "**You rolled : " + dice.ToString() + "**";
                desc += "\n\n**Bot Comment**\n";
                desc += punishment;

                emb.Description = desc;
                await Context.Channel.SendMessageAsync("", false, emb);
                RexTimers.resetTimer(username, "beg");
            }
            else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "beg") + "`");
            }
        }

        [Command("oldbeg")]
        [Alias("obeg","ob","beg")]
        [Remarks("troll")]
        [Summary("Has a chance to forgive you or give you a worse punishment")]
        public async Task obegCmd()
        {
            string username = Context.User.ToString();

            if (!DataUtils.reports.ContainsKey(username))
            {
                await Context.Channel.SendMessageAsync("Not reported yet");
                return;
            }

            if (RexTimers.canRunCmd(username, "beg"))//|| MasterUtils.ContainsAny(username, GlobalVars.ADMINS)
            {
                double dice = DataUtils.rnd.Next(1, 101);
                string punishment = "";
                int oldReportCount = DataUtils.reports[username];
                int newReportCount = oldReportCount;


                if (dice < 21)
                {
                    newReportCount = oldReportCount + 5;
                    punishment = "You don't deserve anything good";
                }
                else if (dice < 41)
                {
                    newReportCount = oldReportCount - 7;
                    punishment = "I'm feeling generous";
                }
                else if (dice < 51)
                {
                    newReportCount = oldReportCount + 10;
                    punishment = "Haha middle finger up your bum";
                }
                else if (dice < 61)
                {
                    newReportCount = oldReportCount - 13;
                    punishment = "You better start to like me";
                }
                else if (dice < 71)
                {
                    newReportCount = oldReportCount + 20;
                    punishment = "gfi idiot";
                }
                else if (dice < 81)
                {
                    newReportCount = oldReportCount - 25;
                    punishment = "you lucky bastard";
                }
                else if (dice < 86)
                {
                    newReportCount = oldReportCount + 35;
                    punishment = "hahaha u failed so hard";
                }
                else if (dice < 91)
                {
                    newReportCount = oldReportCount - 42;
                    punishment = "That's a lot of reports gone you lucky mf";
                }
                else if (dice < 94)
                {
                    newReportCount = oldReportCount *2;
                    punishment = "HAHA in yo face idiot";
                }
                else if (dice < 97)
                {
                    newReportCount = oldReportCount /2;
                    punishment = "its your lucky day m8";
                }
                else if (dice < 99)
                {
                    newReportCount = oldReportCount + 3;
                    punishment = "Woops";
                }
                else if (dice == 99)
                {
                    newReportCount = 0;
                    punishment = "You're a zero not a hero";
                }
                else
                {
                    newReportCount = oldReportCount * 5;
                    punishment = "YOU JUST HIT THE REPORT JACKPOT";
                }

                if (newReportCount > GlobalVars.MAX_REPORTS)
                {
                    newReportCount = 0;
                    DataUtils.gainCoins(username, GlobalVars.MAX_REPORTS);
                    await Context.Channel.SendMessageAsync("Gratz! You went above the report cap. Auto exchanged reports to coins!", true);
                    punishment += "\nGratz! You went above the report cap. Auto exchanged reports to coins!";
                }
                if (newReportCount < 0)
                {
                    newReportCount = 0;
                    punishment += "\nBtw your report count went below 0 so I set it to 0";
                }

                punishment += "\n\n**Report count : " + oldReportCount + " -> " + newReportCount + "**";
                DataUtils.setReports(username, newReportCount);


                EmbedBuilder emb = new EmbedBuilder();
                emb.Color = new Color(255, 0, 255);

                emb.Title = "**Old Beg Info**\n";
                string desc = "1-20 : +5 reports\n";
                desc += "21-40 : -7 reports\n";
                desc += "41-50 : +10 reports\n";
                desc += "51-60 : -13 reports\n";
                desc += "61-70 : +20 reports\n";
                desc += "71-80 : -25 reports\n";
                desc += "81-85 : +35 reports\n";
                desc += "86-90 : -42 reports\n";
                desc += "91-93 : double reports\n";
                desc += "94-96 : half reports\n";
                desc += "97-98 : triple reports\n";
                desc += "99 : set report count to 0\n";
                desc += "100 : Quintuple report count\n\n";
                desc += "**You rolled : " + dice.ToString() + "**";
                desc += "\n\n**Bot Comment**\n";
                desc += punishment;

                emb.Description = desc;
                await Context.Channel.SendMessageAsync("", false, emb);
                RexTimers.resetTimer(username, "beg");
            }
            else
            {
                await Context.Channel.SendMessageAsync("`" + RexTimers.getWaitMsg(username, "beg") + "`");
            }
        }

    }
}