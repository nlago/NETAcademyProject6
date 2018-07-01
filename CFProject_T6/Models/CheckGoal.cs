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

        public static void Getmails(Projects Project, List<Users> Users)
        {
            
            if (Project.Goalfunds <= Project.Fundsrecv)
            {
                MailAddressCollection collection = new MailAddressCollection();
                foreach (var item in Users)
                {
                    collection.Add(item.Email);
                }

                var client = new SmtpClient();

                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("team6dotnet@gmail.com", "123!@#qQ");

                foreach (var user in collection)
                {
                    var to = user;
                    var from = new MailAddress("Team6@Project.gr");
                    var message = new MailMessage(from, to);
                    message.Subject = "Goal Reached";
                    message.Body = "The Project " + Project.Title + " has reached the Goal Funds !!";
                    message.IsBodyHtml = false;

                    client.Send(message);
                }
                
            }
            
        }
        
    }
}
