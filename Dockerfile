FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CourseSystem.sln .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY src/CourseSystem.API/CourseSystem.API.csproj CourseSystem.API/
COPY src/CourseSystem.Application/CourseSystem.Application.csproj CourseSystem.Application/
COPY src/CourseSystem.Infrastructure/CourseSystem.Infrastructure.csproj CourseSystem.Infrastructure/
COPY src/CourseSystem.Persistence/CourseSystem.Persistence.csproj CourseSystem.Persistence/
COPY src/CourseSystem.Exceptions/CourseSystem.Exceptions.csproj CourseSystem.Exceptions/


RUN dotnet restore CourseSystem.sln

COPY . .

RUN dotnet publish CourseSystem.API/CourseSystem.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CourseSystem.API.dll"]