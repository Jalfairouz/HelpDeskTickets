using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }= new List<Ticket>();
    }
}
//relation between Department and Ticket is one-to-many, where one Department can have many Tickets