# Define the SDK base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 

# Set the working directory in the container
WORKDIR /src

# Copy the project files to the container
COPY . .  

# Restore dependencies and build the project
RUN dotnet restore  
RUN dotnet publish -c Release -o /app/publish  

# Define the runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime  

# Set the working directory in the container
WORKDIR /app

# Copy the published build from the SDK base image to the runtime base image
COPY --from=build /app/publish .

# Define the entry point to run the project
ENTRYPOINT ["dotnet", "CryptoAPI.dll"]  
