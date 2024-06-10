using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using AfricasTalkingCS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhoneNumbers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prema.Library.BulkSms
{
    public class BulkSms
    {
        //private readonly AppSetting settings;
        //private readonly Logging logging;
        //public BulkSms(IOptionsMonitor<AppSetting> settings, Logging logging)
        //{
        //    this.settings = settings.CurrentValue;
        //    this.logging = logging;
        //}

        public bool SendSms(string contact, string message, string username, string apiKey)
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

        public bool SendSms(List<string> contacts, string message, string username, string apiKey)
        {
            var msg = message;
            bool status = false; 

            foreach (var recep in contacts)
            {
                status = SendSms(recep, msg, username, apiKey); //flawed status logic needs work.
            }

            return status;
        }

    }
}