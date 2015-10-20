using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WebHttpBehaviorExtensions
{
    public class TypedUriTemplateBehavior : WebHttpBehavior
    {
        public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                if (operation.Behaviors.Contains(typeof(TypeCastOperationBehavior)))
                    continue;

                operation.Behaviors.Add(new TypeCastOperationBehavior());
            }

            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            if (operationDescription.SyncMethod.CustomAttributes.Any(x => x.AttributeType == typeof(UriTemplateSafeAttribute)))
            {
                foreach (var item in operationDescription.Messages[0].Body.Parts)
                {
                    item.Type = typeof(string);
                }
            }

            return base.GetRequestDispatchFormatter(operationDescription, endpoint);
        }
    }
}
