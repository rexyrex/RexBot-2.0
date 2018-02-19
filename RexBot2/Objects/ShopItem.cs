using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2.Objects
{
    public class ShopItem
    {
        public string Name;
        public int Cost;
        public string Syntax;
        public string Description;
        public string Callname;
        public int Argcount;

        public ShopItem(string callname, int argcount,string name, int cost, string syntax, string description)
        {
            Callname = callname;
            Name = name;
            Cost = cost;
            Syntax = syntax;
            Description = description;
            Argcount = argcount;
        }

    }
}