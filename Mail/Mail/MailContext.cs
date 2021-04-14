using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail
{
    public class MailContext : DbContext
    {
        public MailContext() : base("name=MailEntities") { }
        public DbSet<login_user> login_user { get; set; }
        public DbSet<mail_send_user> mail_send_user { get; set; }
        public DbSet<mail_get_user> mail_get_user { get; set; }
        public DbSet<mail_send_document> mail_send_document { get; set; }
        public DbSet<mail_get_document> mail_get_document { get; set; }
    }
}
