using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adverthouse.Core.Notification
{
    public class EmailData
    {
        public Dictionary<string, string> MergeTags { get; set; }
        public string RawContent { get; private set; }
        public string ID { get; private set; }
        public string RawSubject { get; private set; }
        public string EmailBody
        {
            get
            {
                string tempContent = RawContent;
                foreach (var mergeTag in MergeTags)
                {
                    tempContent = tempContent.Replace(String.Format("|-{0}-|", mergeTag.Key), mergeTag.Value);
                }

                return tempContent;
            }
        }
        public string EmailSubject
        {
            get
            {
                string tempSubject = RawSubject;
                foreach (var mergeTag in MergeTags)
                {
                    tempSubject = tempSubject.Replace(String.Format("|-{0}-|", mergeTag.Key), mergeTag.Value);
                }

                return tempSubject;
            }
        }
        public string ToAddress { get; private set; }
        public string CCAddresses { get; set; }
        public string Attachment { get; set; }
        public EmailData(string id, string toAddress, string subject, string rawContent)
        {
            ID = id;
            ToAddress = toAddress;
            RawSubject = subject;

            MergeTags = new Dictionary<string, string>();
            RawContent = rawContent; 
        } 

        public EmailData(string id, string toAddress, string subject, string fileName, bool fromFile)
        {
            ID = id;
            ToAddress = toAddress;
            RawSubject = subject;

            MergeTags = new Dictionary<string, string>();
 
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var sr = new StreamReader(fs, Encoding.Default);
                RawContent = sr.ReadToEnd();
                sr.Close();
            }
        } 
    }
}
