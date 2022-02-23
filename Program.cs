using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;

SmtpSender sender = new SmtpSender(() => new SmtpClient(host: "smtp.gmail.com")
{
    EnableSsl = true,
    UseDefaultCredentials = false,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    Credentials = new NetworkCredential("email", "password"), //TODO: add real user/password here
    Port = 587
});

Email.DefaultSender = sender;
IFluentEmail newEmail = Email
    .From("from_address") //TODO: Update From/To addresses here
    .To("to_address")
    .Subject("Attaching PDF repro")
    .Body("body text");

var attachments = new List<FluentEmail.Core.Models.Attachment>();

var pdfList = new List<string>
{
    "./1.pdf",
    "./2.pdf",
    "./3.pdf"
};

foreach (string path in pdfList)
{

    string fileName = Path.GetFileName(path);

    var attachment = new FluentEmail.Core.Models.Attachment
    {

        Data = File.OpenRead(path),
        ContentType = "application/pdf",
        Filename = $"{fileName}"
    };

    attachments.Add(attachment);
    Console.WriteLine(path);
}

newEmail.Attach(attachments);
attachments.Clear();
FluentEmail.Core.Models.SendResponse result = await newEmail.SendAsync();