FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /api

COPY . .

CMD ["dotnet", "run"]