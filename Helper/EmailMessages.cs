using System.Text;

namespace Api.Helper
{
    public static class EmailMessages
    {
        public static string CreateForgotPasswordMessage(string clearTextPassword)
        {
            return $"<h2>Neues Passwort FelchenApp</h2><br>" +
                   $"<p>Neues Passwort: <b>{clearTextPassword}</b><br>" +
                   $"<p>Bitte ändere das Passwort beim nächsten Login.</p><br>" +
                   $"<p>Freundliche Grüsse</p>" +
                   $"<p>FelchenApp Support</p>" +
                   $"<br>" +
                   $"<small>Diese E-Mail wurde automatisch generiert, bitte nicht auf diese E-Mail Antworten</small>";
        }

        public static string CreateContactMessage(Contact contact)
        {
            return $"<h2>Kontakt von FelchenApp</h2><br><br><p>Email von {contact.Email}</p>" +
                   $"<p>{GetText(contact.Message)}</p>";
        }
        
        private static string GetText(string messageContent)
        {
            var stringBuilder = new StringBuilder();
            var lines = messageContent.Split("\n");
            foreach (var line in lines)
            {
                stringBuilder.Append(line + "<br>\n");
            }

            return stringBuilder.ToString();
        }
    }
}