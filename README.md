# WebHttpBehaviorExtensions

[![wcfwebhttpbehaviorextensions MyGet Build Status](https://www.myget.org/BuildSource/Badge/wcfwebhttpbehaviorextensions?identifier=5172fa2f-2633-4a60-aee4-f554740f8da9)](https://www.myget.org/)  
                    [![wcfwebhttpbehaviorextensions Nuget package Status](https://img.shields.io/nuget/vpre/WebHttpBehaviorExtensions.svg)](https://www.nuget.org/packages/WebHttpBehaviorExtensions)

### List of Behaviors the library provides
- EnableCorsBehavior
- CustomAuthenticationBehavior
- TypedUriTemplateBehavior

## EnableCorsBehavior
This behavior extension can be used to enable the CORS headers in WCF service. The behavior works in both IIS and stand alone WCF hosting. 
### Usage
- Downlaod and Install the Nuget package using NPM `Install-Package WebHttpBehaviorExtensions -Pre`
- You can also download this library and reference it in your code and use directly. 
 Register behavior extension in your WCF service configuration file.
```xml
<extensions>
      <behaviorExtensions>
        <add name="corsBehavior" type="WebHttpBehaviorExtensions.Cors.EnableCorsBehavior, WebHttpBehaviorExtensions, Version=1.0.0.0, Culture=neutral"/>
      </behaviorExtensions>
</extensions>
```
Add the behavior in EndpointBehaviors

```xml
 <endpointBehaviors>
    <behavior name="webHttpServiceBehavior">
      <corsBehavior/>
      <webHttp/>
    </behavior>
</endpointBehaviors>
```
Use the EndPointBehavior in your Endpoint 
```xml
<endpoint binding="webHttpBinding" contract="WcfWebHttpIISHostingSample.ITestService" behaviorConfiguration="webHttpServiceBehavior"/>
```
This is it. Your WCF REST Api is now ready to accept cross domain requests.

## CustomAuthenticationBehvior
The extension class in Security namespace provides a way to inject your own custom authentication mechanism in WCF REST services. You can inject your custom logic and use Basic Authentication/OAuth authentication or any other cusotm logic to secure your WCF Service without any hassel. 

>**Why creating an extension when we have in-built authentication\authorization available in WCF?**
The challenge people face is when WCF REST service is hosted on IIS and you need to use Basic Authentication. The problem is that IIS applies it's own Basic Authentication before request reaches to WCF framework. For more details about this issue read Allen Conway's blog on [Basic Authentication challenges in WCF hosted in IIS](http://www.allenconway.net/2012/07/using-basic-authentication-in-rest.html, "Basic Authentication challenges in WCF hosted in IIS").
Namespace `WebHttpBehaviorExtensions.Security` have all the components of this extension. 

### Usage
- Downlaod and Install the Nuget package using NPM `Install-Package WebHttpBehaviorExtensions -Pre`
- You can also download this library and reference it in your code and use directly. 
- Add custom RestAuthorizationManager to your WCF configuration. 
```xml
    <behaviors>
      <serviceBehaviors>
        <behavior name="RestServiceBehavior">
          <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true"/>
          <serviceDebug includeExceptionDetailInFaults="true"/>
          <serviceAuthorization 
            serviceAuthorizationManagerType
              ="WebHttpBehaviorExtensions.Security.RestAuthorizationManager, WebHttpBehaviorExtensions"/>
        </behavior>
      </serviceBehaviors>
    </behaviors>
```
- Implement the `WebHttpBehaviorExtensions.Security.IAuthenticationProvider` interface and provide your custom logic in `Authenticate` method.

#### Sample Basic Authentication implemenation using `IAuthenticationProvider`
```csharp
    public class BasicAuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(System.ServiceModel.OperationContext operationContext)
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            AuthenticationHeader header;

            if (AuthenticationHeader.TryDecode(authHeader, out header))
            {
                /* Demo credetentials validation*/
                if ((header.Username == "testuser" && header.Password == "testpassword"))
                {
                    //User is authrized and originating call will proceed
                    return true;
                }
                else
                {
                    //not authorized
                    return false;
                }
            }
            else
            {
                //No authorization header was provided, so challenge the client to provide before proceeding:
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\" WcfWebHttpIISHostingSample\"");
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
```
- Finally register new  AuthenticationProvider with  with your application startup point. In case of IIS hosting registration will happen in `Application_Start` event. 

```csharp
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(TestService)));
            AuthContext.SetAuthenticationProvider(() => new BasicAuthenticationProvider());
        }
```
- Run and test your WCF REST service. You'll see a basic authentication challenge received in response for any request.

- For OAuth sample see `WcfWebHttpIISHostingSample.OAuthAuthenticationProvider` in project WcfWebHttpIISHostingSample in the library source code.
- For Enabling SSL in WCF REST service see `tutorial guide` under `docs` in source code.

## TypedUriTemplateBehavior 
This repository is a simple WebHttpBehavior extension that will allow Typed arguments for method via UriTemplates in WCF Rest services.

>In WCF the WebHttpBinding for REST support are restricted to use Types arguments in contract operations which are marked with UriTemplates. This custom library provides a custom WebHttpBehavior that overrids the run time checks and allow other types in method signatures. The Idea came from one of the question asked on [Stackoverflow here](http://stackoverflow.com/questions/33018220/how-can-i-use-strongly-typed-parameters-in-the-uri-path-in-wcf-with-webhttpbindi, "How can I use strongly typed parameters in the uri path in WCF with WebHttpBinding").


The source code contains core library `WebHttpBehaviorExtensions`, A sample WCF service host and run time tests and a Unit tests project `WebHttpBehaviorExtensions.Tests`

### Usage
Default provided method operations in WebHttpBindings(REST) are restricted to string usage only if you use URI template
```csharp
        [WebInvoke(Method = "GET", UriTemplate = "/Data/{data}")]
        string GetData(string data);
```
But if you apply the behavior `TypedUriTemplateBehavior` provided in the core library. This will allow method declarations to have other types that can be represeted as string and automatically cast run time objects to their respective types. To enable the behavior on selected operations mark them with attribute `UriTemplateSafeAttribute` see below samples:

- Downlaod and Install the Nuget package using NPM `Install-Package WebHttpBehaviorExtensions -Pre`
- You can also download this library and reference it in your code and use directly. 

```csharp
        [WebInvoke(Method = "GET", UriTemplate = "/{id}")]
        [UriTemplateSafe]
        string GetValue(Guid id);

        [WebInvoke(Method = "GET", UriTemplate = "/Parent/{id}")]
        [UriTemplateSafe]
        string GetParentId(int id);


        [WebInvoke(Method = "GET", UriTemplate = "/{guid}/Current/{id}")]
        [UriTemplateSafe]
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
> **Note** - *If you have additional CustomBehavior which inherits WebHttpBehavior; the order of registration of behavior in EndPointBehavior might override others*. 

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
