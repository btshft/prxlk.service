FROM microsoft/dotnet:aspnetcore-runtime as base
WORKDIR /app

FROM microsoft/dotnet:sdk AS build
WORKDIR /src

COPY ["/Prxlk.IdentityService/*.csproj", "Prxlk.IdentityService/"]

RUN dotnet restore "Prxlk.IdentityService/Prxlk.IdentityService.csproj"

COPY . .
WORKDIR "/src/Prxlk.IdentityService"
RUN dotnet build "Prxlk.IdentityService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Prxlk.IdentityService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Not working for heroku
# ENTRYPOINT ["dotnet", "Prxlk.IdentityService.dll"]

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Prxlk.IdentityService.dll