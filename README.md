# TrackED
## Software Engineering 1 Project for Fall 2025

### Team Members:
- Daniel Kefene
- Katherine King
- Mason Moore
- Will Paxton
- Tyler Stephens
- Nick Trahan

### Project Overview
- Tracks nursing students during clinicals​
- Helps professors monitor progress & safety​
- Central dashboard for oversight and tasks​

### Vision
- Improve student safety & accountability​
- Real-time geolocation tracking​
- Role-based login system​
- Analytics dashboard​
- Clinical task management​
- Support ETSU Nursing Program with a centralized tracking system​
- Increase safety + accountability during clinical rotations​
- Provide quick access to student progress & task completion​
- Simplify coordination between professors and students​

### Launching the Application

First, the .NET 8.0 SDK is a prerequisite to running this app.  You can download it at https://dotnet.microsoft.com/en-us/download/dotnet/8.0.

To open and run code:

launch "TrackEd.sln" to open in Visual Studio Community

launch "VSCode_Launch.bat" to open in VS Code

Run "VSCode_Compiler.bat" to compile code and launch an auto-monitoring test website or optionally run `dotnet watch run --urls "https://0.0.0.0:5287"` from the TrackEd folder to host locally on port 5287 with hot-reload enabled for a development environment.  If hot-reload is not needed or this program is running in production, drop “watch” from the prior command and instead run `dotnet run --urls https://0.0.0.0:5287`.  Ensure that you connect to the website over HTTPS, or geolocation functionality will not work.  
