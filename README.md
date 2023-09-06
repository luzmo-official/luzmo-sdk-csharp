
## XUnit Unit tests  

```sh
cd CumulioSDK.Test
dotnet restore
dotnet build
dotnet test
```

## Pushing to Nuget registry  

Usually you have to call pack to create nupkg, but I changed csproj configuration to remove that.  


Get API Key from https://www.nuget.org/account/apikeys , sign in using developer_cumulio@outlook.com account.

```sh
cd LuzmoSDK
dotnet build --configuration Release
dotnet nuget push ./bin/Release/LuzmoSDK.1.0.1.nupkg --api-key API_KEY --source https://api.nuget.org/v3/index.json
```


## How the project was created

```
dotnet new sln
dotnet new classlib -o LuzmoSDK
dotnet new xunit -o LuzmoSDK.Test
dotnet new console -o ExampleApp
dotnet sln add LuzmoSDK
dotnet sln add LuzmoSDK.Test
dotnet sln add ExampleApp
cd LuzmoSDK.Test
dotnet add reference ../LuzmoSDK
cd ../ExampleApp
dotnet add reference ../LuzmoSDK
cd ..
dotnet restore
dotnet build
```