using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Douyu.Gift
{
    public class Gift
    {
        public Gift(int id, string name, string type, double price, double experience, string desc, string intro,
            string ming, string himg)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Experience = experience;
            Desc = desc;
            Intro = intro;
            Mimg = ming;
            Himg = himg;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public double Price { get; private set; }
        public double Experience { get; private set; }
        public string Desc { get; private set; }
        public string Intro { get; private set; }
        public string Mimg { get; private set; }
        public string Himg { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", this.Name, this.Id);
        }
    }
}
