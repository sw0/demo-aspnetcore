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