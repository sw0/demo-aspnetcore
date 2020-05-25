# demo-aspnetcore
aspnetcore demos

# Initial a solution with AspNetCore WebAPI project
```
cd C:\Users\Shawn\source\github\demo-aspnetcore

# generate NetCoreDemo.sln file
dotnet new sln --name NetCoreDemo

# create webapi project
dotnet new webapi -o WebApiDemo

dotnet sln add .\WebApiDemo\WebApiDemo.csproj
 ```

 # Add PoemController and PoemViewModel
 With default N Poem Items just initialized in PoemController.
 - Get
 - Get One
 - Post to add one
 - Put to modify one
 - Delete to remove one
 - api/Poem/author/libai to use `PhysicalFile` to send physical file

# Swagger
- [Swashbuckle.Swagger and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)

## Intall Packages
- Swashbuckle.AspNetCore
  - Swashbuckle.AspNetCore.Swagger
  - Swashbuckle.AspNetCore.SwaggerGen
  - Swashbuckle.AspNetCore.SwaggerUI
## Add and configure swagger middleware
https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio#add-and-configure-swagger-middleware
## Customize and extend
* Xml Document (Summary, Remarks, ResponseCode)
* [Produces("application/json")]
* [ProduceResponseType(StatusCodes.Status201Created, typeof(Article)]
