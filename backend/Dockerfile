FROM mcr.microsoft.com/dotnet/sdk:10.0-preview
WORKDIR /app

COPY . .

EXPOSE 5000

CMD ["dotnet", "run", "--project", "src/GastosResidenciais.API", "--urls", "http://+:5000"]
