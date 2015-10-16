using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace WebHttpBehaviorExtensions
{
    /// <summary>
    /// The class must be implemented in order to use it with the custom behavior in configuration file.
    /// </summary>
    public class TypedUriTemplateBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(TypedUriTemplateBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new TypedUriTemplateBehavior();
        }
    }

    public class TypedUriTemplateBehaviorWebContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}
