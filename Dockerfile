FROM node:18 AS frontend-build
WORKDIR /frontend

COPY src/CourseSystem.Frontend/package*.json ./
RUN npm install

COPY src/CourseSystem.Frontend/ ./
RUN npm run build     # Produces "dist" folder

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CourseSystem.sln .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY src/ ./src
COPY tests/ ./tests

RUN dotnet restore CourseSystem.sln
RUN dotnet publish src/CourseSystem.API/CourseSystem.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

COPY --from=frontend-build /frontend/dist ./wwwroot/

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CourseSystem.API.dll"]
