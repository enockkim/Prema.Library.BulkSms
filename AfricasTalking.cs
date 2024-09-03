using AfricasTalkingCS;
using Newtonsoft.Json.Linq;

namespace Prema.Library.BulkSms
{
    public class BulkSms
    {
        public static string apiKey = "";
        public static bool SendSms(string contact, string message, string username)
        {
            var recep = contact;
            var msg = message;

            var gateway = new AfricasTalkingGateway(username, apiKey);

            try
            {
                var res = gateway.SendMessage(recep, msg);

                JToken root = JToken.FromObject(res);

                JArray recipientsArray = (JArray)root["SMSMessageData"]["Recipients"];

                foreach (JToken recipient in recipientsArray)
                {
                    string number = recipient["number"].ToString();
                    string status = recipient["status"].ToString();

                    //logging.WriteToLog($"Number: {number}, Status: {status}", "Information");
                }

                return true;
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine(exception);
                //logging.WriteToLog($"AfricasTalkingGatewayException, SendSms: {exception}", "Error");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //logging.WriteToLog($"SendSms: {ex}", "Error");
                return false;
            }
        }

        public static bool SendMultipleSms(List<string> recipients, string message, string username)
        {
            var msg = message;
            bool status = false; 

            foreach (var recipient in recipients)
            {
                status = SendSms(recipient, msg, username); //flawed status logic needs work.
            }

            return status;
        }

        public static bool SendSIngleSms(string recipient, string message, string username)
        {
            var msg = message;
            bool status = false;

            status = SendSms(recipient, msg, username);

            return status;
        }


    }
}