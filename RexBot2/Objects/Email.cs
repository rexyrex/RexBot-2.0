using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2.Objects
{
    public class Email
    {
        public string sender;
        public string contents;

        public Email(string sender, string contents)
        {
            this.contents = contents;
            this.sender = sender;
        }

        public string getTimePassedSinceSent()
        {
            return "";
        }
    }
}
