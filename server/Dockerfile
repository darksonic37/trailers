FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
WORKDIR /app
COPY . .
RUN dotnet restore
EXPOSE 5000-5001
ENTRYPOINT [ "dotnet", "restore", "&&", "dotnet", "watch", "run", "--no-restore", "--urls", "https://0.0.0.0:5000"]
