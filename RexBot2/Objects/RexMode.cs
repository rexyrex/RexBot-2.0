using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RexBot2.Objects
{
    public class RexMode
    {
        string name;
        string description;
        string[] permissions;

        public RexMode(string n, string d, string[] p)
        {
            name = n;
            description = d;
            permissions = p;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string[] getPermissions()
        {
            return permissions;
        }

        public int getTriggerChance()
        {
            int chance = 0;
            foreach(string permission in permissions)
            {
                if (permission.Contains("trigger"))
                {
                    chance = int.Parse(permission.Split()[1]);
                }
            }
            return chance;
        }

        public bool hasPermission(string perm)
        {
            foreach (string permission in permissions)
            {
                if (permission.Contains(perm))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
