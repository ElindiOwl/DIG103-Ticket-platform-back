FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY DIG103-Ticket-platform-back /app
COPY *.csproj /app/
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o out
EXPOSE 8080
ENTRYPOINT ["dotnet", "out/DIG103-Ticket-platform-back.dll"]
