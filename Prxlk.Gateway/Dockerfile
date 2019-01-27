FROM microsoft/dotnet:aspnetcore-runtime as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:sdk AS build
WORKDIR /src

COPY ["/Prxlk.Application/*.csproj", "Prxlk.Application/"]
COPY ["/Prxlk.ComponentRegistrar/*.csproj", "Prxlk.ComponentRegistrar/"]
COPY ["/Prxlk.Contracts/*.csproj", "Prxlk.Contracts/"]
COPY ["/Prxlk.Data.EntityFramework/*.csproj", "Prxlk.Data.EntityFramework/"]
COPY ["/Prxlk.Domain/*.csproj", "Prxlk.Domain/"]
COPY ["/Prxlk.Gateway/*.csproj", "Prxlk.Gateway/"]
COPY ["/Prxlk.Shared/*.csproj", "Prxlk.Shared/"]

RUN dotnet restore "Prxlk.Gateway/Prxlk.Gateway.csproj"

COPY . .
WORKDIR "/src/Prxlk.Gateway"
RUN dotnet build "Prxlk.Gateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Prxlk.Gateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Prxlk.Gateway.dll"]