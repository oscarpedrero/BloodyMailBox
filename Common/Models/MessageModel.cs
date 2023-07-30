﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyMailBox.Common.Models
{
    internal class MessageModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public bool Open { get; set; }

    }
}
