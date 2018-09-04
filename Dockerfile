FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY CSEvaluator/*.csproj ./CSEvaluator/
RUN dotnet restore

# copy everything else and build app
COPY CSEvaluator/. ./CSEvaluator/
WORKDIR /app/CSEvaluator
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/CSEvaluator/out ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CSEvaluator.dll
