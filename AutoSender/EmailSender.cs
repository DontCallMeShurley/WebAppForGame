﻿using System.Net.Mail;
using System.Net;
using WebAppForGame.Data;

public sealed class EmailSender
{ 
    public static void SendEmail(List<log_gameover> logs)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("370716@edu.itmo.ru");
        mail.To.Add(new MailAddress("dmitriy@waznaw.ru"));
        mail.To.Add(new MailAddress("mcitylam@gmail.com"));
        mail.To.Add(new MailAddress("lord1of2ultima@gmail.com"));
        mail.Subject = $"Количество прохождения за {DateTime.Today.ToString("yyyy.MM.dd")} - {logs.Count}";
        mail.IsBodyHtml = true;

        string data = "";
        logs.ForEach(x => data += $"{x.user_id} прошёл {x.Date.ToString("yyyy.MM.dd HH:mm:ss")} счёт: <b>{x.score}</b><br/>");


        mail.Body = data;

        SmtpClient client = new SmtpClient();
        client.Host = "smtp.yandex.ru";
        client.Port = 587;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("370716@edu.itmo.ru", "aQs2dEf4!");

        client.Send(mail);


    }
}