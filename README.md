# WebHttpBehaviorExtensions

[![wcfwebhttpbehaviorextensions MyGet Build Status](https://www.myget.org/BuildSource/Badge/wcfwebhttpbehaviorextensions?identifier=5172fa2f-2633-4a60-aee4-f554740f8da9)](https://www.myget.org/)  
                    [![wcfwebhttpbehaviorextensions Nuget package Status](https://img.shields.io/nuget/vpre/WebHttpBehaviorExtensions.svg)](https://www.nuget.org/packages/WebHttpBehaviorExtensions)

### List of Behaviors the library provides
- EnableCorsBehavior
- TypedUriTemplateBehavior

(TODO: Add documentation for usage of EnableCorsBehavior)

This repository is a simple WebHttpBehavior extension that will allow Typed arguments for method via UriTemplates in WCF Rest services.

>In WCF the WebHttpBinding for REST support are restricted to use Types arguments in contract operations which are marked with UriTemplates. This custom library provides a custom WebHttpBehavior that overrids the run time checks and allow other types in method signatures. The Idea came from one of the question asked on [Stackoverflow here](http://stackoverflow.com/questions/33018220/how-can-i-use-strongly-typed-parameters-in-the-uri-path-in-wcf-with-webhttpbindi, "How can I use strongly typed parameters in the uri path in WCF with WebHttpBinding").


The source code contains core library `WebHttpBehaviorExtensions`, A sample WCF service host and run time tests and a Unit tests project `WebHttpBehaviorExtensions.Tests`

## Usage

Default provided method operations in WebHttpBindings(REST) are restricted to string usage only if you use URI template
```csharp
        [WebInvoke(Method = "GET", UriTemplate = "/Data/{data}")]
        string GetData(string data);
```
But if you apply the behavior `TypedUriTemplateBehavior` provided in the core library. This will allow method declarations to have other types that can be represeted as string and automatically cast run time objects to their respective types.

```csharp
        [WebInvoke(Method = "GET", UriTemplate = "/{id}")]
        string GetValue(Guid id);

        [WebInvoke(Method = "GET", UriTemplate = "/Parent/{id}")]
        string GetParentId(int id);


        [WebInvoke(Method = "GET", UriTemplate = "/{guid}/Current/{id}")]
        string GetCurrentTime(Guid guid, int id);
```
#### Adding custom behavior to WCF WebHttp binding via configuration file
To add this behavior you need to add custom behavior extension in the webconfiguration for Endpoint behavior.
```xml
    <extensions>
      <behaviorExtensions>
        <add name="customWebHttpBehavior" type="WebHttpBehaviorExtensions.TypedUriTemplateBehaviorExtension, WebHttpBehaviorExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=Null"/>
      </behaviorExtensions>
    </extensions>
```
Add declared behavior extension to endpoint behaviors
```xml
      <endpointBehaviors>
        <behavior name="webHttpServiceBehavior">
          <customWebHttpBehavior/>
          <webHttp/>
        </behavior>
      </endpointBehaviors>
```
Add bindings(optional)
```xml
  <bindings>
      <webHttpBinding>
        <binding name="customHttpBinding" contentTypeMapper="WebHttpBehaviorExtensions.TypedUriTemplateBehaviorWebContentTypeMapper, WebHttpBehaviorExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=Null"/>
      </webHttpBinding>
    </bindings>
```

Finally attach the behavior with your service endpoint:
```xml
 <service name="WcfWebHttpIISHostingSample.TestService" behaviorConfiguration="ServiceBehavior">
        <endpoint binding="webHttpBinding" contract="WcfWebHttpIISHostingSample.ITestService" behaviorConfiguration="webHttpServiceBehavior" bindingConfiguration="customHttpBinding"/>
      </service>
```

#### Adding custom behavior to WCF WebHttp binding programmatically

Sample of attaching end point behavior in a console service host.

```csharp
    public class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:5000/Service";
            ServiceHost host = new ServiceHost(typeof(TestService), new Uri(baseAddress));

            var webendpoint = host.AddServiceEndpoint(typeof(ITestService), new WebHttpBinding(), "");
            webendpoint.Behaviors.Add(new TypedUriTemplateBehavior());

            host.Open();
            Console.WriteLine("Host opened");

            WebClient c = new WebClient();

            #region Tests
            Console.WriteLine(c.DownloadString(baseAddress + "/" + Guid.NewGuid().ToString()));

            Console.WriteLine(c.DownloadString(baseAddress + "/Parent/" + 20));

            Console.WriteLine(c.DownloadString(baseAddress + "/" + Guid.NewGuid() + "/Current/" + 10));

            Console.WriteLine(c.DownloadString(baseAddress + "/Data/" + "Mein_name_ist_WCF"));
            #endregion

            Console.Read();
        }
    }
```

Feel free to fork/like/contribute.
