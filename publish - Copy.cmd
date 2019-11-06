nuget restore
msbuild EchoBot.sln -p:DeployOnBuild=true -p:PublishProfile=chatbotsogetidemo-Web-Deploy.pubxml -p:Password=

