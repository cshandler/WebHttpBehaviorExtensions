using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using WebHttpBehaviorExtensions.Helpers;

namespace WebHttpBehaviorExtensions
{
    internal class TypeCastOperationInvoker : IOperationInvoker
    {
        private readonly MethodInfo info;
        private readonly IOperationInvoker _invoker;

        public TypeCastOperationInvoker(IOperationInvoker invoker)
        {
            _invoker = invoker;
        }

        public TypeCastOperationInvoker(IOperationInvoker invoker, MethodInfo info)
            : this(invoker)
        {
            this.info = info;
        }

        public object[] AllocateInputs()
        {
            return _invoker.AllocateInputs().ToArray();
        }

        private object[] CastCorrections(object[] inputs)
        {
            if(!this.info.CustomAttributes.Any(x => x.AttributeType == typeof(UriTemplateSafeAttribute)))
            {
                return inputs;
            }

            var outarray = new object [inputs.Length];

            var paramInfo = this.info.GetParameters();

            for (int i = 0; i < inputs.Length; i++)
            {
                object typedObject;
                var type = paramInfo[i].ParameterType;

                inputs[i].TryConvertTo(type, out typedObject);
                outarray[i] = typedObject;
            }

            return outarray;
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            return _invoker.Invoke(instance, CastCorrections(inputs), out outputs);
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _invoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return _invoker.InvokeEnd(instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get { return _invoker.IsSynchronous; }
        }
    }
}
