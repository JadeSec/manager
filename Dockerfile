#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ=America/New_York

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Presentation/App.Mvc/App.Mvc.csproj", "src/Presentation/App.Mvc/"]
COPY ["src/Application/App.Application.Poc/App.Application.Poc.csproj", "src/Application/App.Application.Poc/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Redis/App.Infra.Integration.Redis.csproj", "src/Infra/Integration/App.Infra.Integration.Redis/"]
COPY ["src/Infra/Bootstrap/App.Infra.Bootstrap/App.Infra.Bootstrap.csproj", "src/Infra/Bootstrap/App.Infra.Bootstrap/"]
COPY ["src/Domain/App.Domain/App.Domain.csproj", "src/Domain/App.Domain/"]
COPY ["src/Application/App.Application.Payment/App.Application.Payment.csproj", "src/Application/App.Application.Payment/"]
COPY ["src/Repositories/App.Repositories/App.Repositories.csproj", "src/Repositories/App.Repositories/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Database/App.Infra.Integration.Database.csproj", "src/Infra/Integration/App.Infra.Integration.Database/"]
COPY ["src/Infra/CrossCutting/App.Infra.CrossCutting/App.Infra.CrossCutting.csproj", "src/Infra/CrossCutting/App.Infra.CrossCutting/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Stripe/App.Infra.Integration.Stripe.csproj", "src/Infra/Integration/App.Infra.Integration.Stripe/"]
COPY ["src/Application/App.Application.Showcase/App.Application.Showcase.csproj", "src/Application/App.Application.Showcase/"]
COPY ["src/Infra/Integration/App.Infra.Integration.SendGrid/App.Infra.Integration.SendGrid.csproj", "src/Infra/Integration/App.Infra.Integration.SendGrid/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Aws/App.Infra.Integration.Aws.csproj", "src/Infra/Integration/App.Infra.Integration.Aws/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Jwt/App.Infra.Integration.Jwt.csproj", "src/Infra/Integration/App.Infra.Integration.Jwt/"]
COPY ["src/Application/App.Application.Share/App.Application.Share.csproj", "src/Application/App.Application.Share/"]
COPY ["src/Infra/Integration/App.Infra.Integration.Plaid/App.Infra.Integration.Plaid.csproj", "src/Infra/Integration/App.Infra.Integration.Plaid/"]
COPY ["src/Application/App.Application.Wallet/App.Application.Wallet.csproj", "src/Application/App.Application.Wallet/"]
RUN dotnet restore "src/Presentation/App.Mvc/App.Mvc.csproj"
COPY . .
WORKDIR "/src/src/Presentation/App.Mvc"
RUN dotnet build "App.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.Mvc.dll"]