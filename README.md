# Luzmo API

You can use this C# project to interact with the [Luzmo](https://luzmo.com) API in order to create, modify or delete datasets, dashboards or push new data into the platform in a programmatic way.

## Usage

See the `ExampleApp/Example.cs` file for examples how to create datasets or push data into the platform (triggering real-time dashboard updates).

## Documentation

The API documentation (available services and methods) can be found [here](https://developer.luzmo.com/).


## XUnit Unit tests  

```sh
cd LuzmoSDK.Test
dotnet restore
dotnet build
dotnet test
```