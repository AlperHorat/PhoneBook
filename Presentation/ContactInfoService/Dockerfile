FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Presentation/ContactInfoService/ContactInfoService.csproj", "Presentation/ContactInfoService/"]
COPY ["Domain/PhoneBook.Domain/PhoneBook.Domain.csproj", "Domain/PhoneBook.Domain/"]
COPY ["Infrastructure/PhoneBook.Infrastructure/PhoneBook.Infrastructure.csproj", "Infrastructure/PhoneBook.Infrastructure/"]
COPY ["Services/PhoneBook.Services/PhoneBook.Services.csproj", "Services/PhoneBook.Services/"]
COPY ["Contracts/PhoneBook.Contracts/PhoneBook.Contracts.csproj", "Contracts/PhoneBook.Contracts/"]

RUN dotnet restore "Presentation/ContactInfoService/ContactInfoService.csproj"

COPY . .
WORKDIR "/src/Presentation/ContactInfoService"
RUN dotnet build "ContactInfoService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactInfoService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ContactInfoService.dll"]