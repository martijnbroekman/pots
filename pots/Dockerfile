#Build the app image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app  
ENV ASPNETCORE_ENVIRONMENT Local
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "pots.dll"]
COPY ./bin/Release/netcoreapp2.1/publish .
EXPOSE 80 443