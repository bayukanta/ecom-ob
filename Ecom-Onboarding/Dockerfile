FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster as build-env
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
COPY . .

RUN dotnet restore "./Ecom-onboarding.DAL/Ecom-Onboarding.DAL.csproj"
RUN dotnet restore "./Ecom-Onboarding.BLL/Ecom-Onboarding.BLL.csproj"
RUN dotnet restore "./Ecom-Onboarding/Ecom-Onboarding.csproj"


WORKDIR /app/Ecom-Onboarding/
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
COPY --from=build-env /app/Ecom-Onboarding/out ./
ENTRYPOINT ["dotnet", "Ecom-Onboarding.dll"]