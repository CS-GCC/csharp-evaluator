FROM microsoft/dotnet:2.1.1-runtime
WORKDIR /app
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CSEvaluator.dll
