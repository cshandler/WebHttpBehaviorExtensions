@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

REM Package restore
call %NuGet% restore src\WebHttpBehaviorExtensions.Tests\packages.config -OutputDirectory %cd%\packages -NonInteractive

REM Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src\WebHttpBehaviorExtensions.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false
if not "%errorlevel%"=="0" goto failure

REM Unit tests
call %nuget% install NUnit.Runners -Version 2.6.4 -OutputDirectory packages
packages\NUnit.Runners.2.6.4\tools\nunit-console.exe /config:%config% /framework:net-4.5 src\WebHttpBehaviorExtensions.Tests\bin\%config%\WebHttpBehaviorExtensions.Tests.dll
if not "%errorlevel%"=="0" goto failure

mkdir Build
mkdir Build\lib
mkdir Build\lib\net45

REM Package
mkdir Build
call %nuget% pack "src\WebHttpBehaviorExtensions\WebHttpBehaviorExtensions.csproj" -symbols -o Build -p Configuration=%config% %version%
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1
