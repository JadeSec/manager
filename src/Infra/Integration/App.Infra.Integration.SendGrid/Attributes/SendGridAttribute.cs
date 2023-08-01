using System;
using System.Reflection;

namespace App.Infra.Integration.SendGrid.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SendGridAttribute : Attribute
    {
        private string _bcc;
        private string _from;
        private string _templateId;

        public SendGridAttribute(string templateId, string from = "", string bcc = "")
        {
            _templateId = templateId;
            _from = from;
            _bcc = bcc;
        }       

        public virtual string From
        {
            get { return _from; }
        }

        public virtual string TemplateId
        {
            get { return _templateId; }
        }

        public virtual string Bcc
        {
            get { return _bcc; }
        }

        public static SendGridAttribute Parse(Type type)
           => type.GetTypeInfo()
                  .GetCustomAttribute<SendGridAttribute>() ?? throw new ArgumentNullException($"Not exist attribute [SendGrid()] in {type.GetType().Name}");
    }
}
