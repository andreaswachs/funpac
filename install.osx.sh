#!/bin/zsh

dotnet clean
dotnet publish -c Release -r osx-x64 /p:PublishSingleFile=true /p:UseAppHost=true
mkdir -p ~/opt/bin/
cp ./Funpac/bin/Release/net6.0/osx-x64/publish/Funpac ~/opt/bin/funpac