using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public static class CheckGoal
    {

        //private readonly ProjectContext _context;

        //public CheckGoal(ProjectContext context)
        //{
        //    _context = context;
        //}

        public static void Getmails(Projects Project, List<Users> Users)
        {

            

            if (Project.Goalfunds <= Project.Fundsrecv)
            {
                MailAddressCollection collection = new MailAddressCollection();
                foreach (var item in Users)
                {
                    collection.Add(item.Email);
                    
                }
                //var users = _context.BackersProjects
                //.Where(b => b.ProjectId == Project.Id)
                //.Select(b => b.User)
                //.ToList();

                foreach (var user in collection)
                {
                    var to = user;

                    var from = new MailAddress("Team6@Project.gr");
                    var message = new MailMessage(from, to);
                    message.Subject = "Goal Reached";
                    message.Body = "The Project " + Project.Title + " has reached the Goal Funds !!";

                    var client = new SmtpClient();

                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "smtp.gmail.com";

                    client.Send(message);
                }


               

            }


           

            var cliento = new SmtpClient();

            cliento.Host = "smtp.gmail.com";
            cliento.Port = 587;
            cliento.EnableSsl = true;
            //NetworkCredential nc = new NetworkCredential("k.perissis@gmail.com", "aeon0202");
            cliento.UseDefaultCredentials = false;
            cliento.Credentials = new System.Net.NetworkCredential("team6dotnet@gmail.com", "123!@#qQ");
            

            var too = new MailAddress("xzyg_kxz@yahoo.com");
            var fromo = new MailAddress("Team6@Project.gr");
            var messageo = new MailMessage(fromo, too);
            messageo.Subject = "Goal Reached";
            messageo.Body = "The Project " + Project.Title + " has reached the Goal Funds !!";
            messageo.IsBodyHtml = false;

            cliento.Send(messageo);
        }
        
    }
}
