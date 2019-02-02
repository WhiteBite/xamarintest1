using System;
using System.Collections.Generic;
using System.Text;

namespace App5.Models
{
    public class Offer
    {
        public Offer(int id, string str)
        {
            Jsonstr = str;
            this.id = id.ToString();
        }
        private string id;
        private string jsonstr;

        public string Id { get => id; set => id = value; }
        public string Jsonstr { get => jsonstr; set => jsonstr = value; }
    }
}
