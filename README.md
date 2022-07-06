
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
cd CumulioSDK
dotnet build --configuration Release
dotnet nuget push ./bin/Release/CumulioSDK.1.0.1.nupkg --api-key API_KEY --source https://api.nuget.org/v3/index.json
```


## How the project was created

```
dotnet new sln
dotnet new classlib -o CumulioSDK
dotnet new xunit -o CumulioSDK.Test
dotnet new console -o ExampleApp
dotnet sln add CumulioSDK
dotnet sln add CumulioSDK.Test
dotnet sln add ExampleApp
cd CumulioSDK.Test
dotnet add reference ../CumulioSDK
cd ../ExampleApp
dotnet add reference ../CumulioSDK
cd ..
dotnet restore
dotnet build
```