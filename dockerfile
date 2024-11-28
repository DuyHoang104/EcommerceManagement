# Use the SDK image to build the app
FROM bitnami/dotnet-sdk AS build
WORKDIR /app
COPY EcommerceManagement.sln .
COPY ./src ./src
RUN dotnet restore "EcommerceManagement.sln"
RUN dotnet build
RUN dotnet publish -c Release -o /out

# Copy the build output to the runtime image
FROM bitnami/aspnet-core AS final
WORKDIR /publish
EXPOSE 5001
COPY --from=build ./out .
ENTRYPOINT [ "dotnet", "EcommerceManagement.Web.dll", "--urls", "http://172.18.0.2:5001/"]