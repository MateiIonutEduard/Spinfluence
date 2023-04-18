## Description

This application is used to manage practices of students at IT companies.<br/>

## Project Setup
Before running the application, need to download [XAMPP Server](https://www.apachefriends.org/download.html) to use MySQL.<br>
After that create the database from the included migration scheme, open Nuget Package Manager.<br><br><br>
<b>Package Manager Console</b>
```powershell
PM> Update-Database
PM> Remove-Migration
```
<br/>
<b>Otherwise, open the terminal:</b>
<br/><br/>

```shell

> dotnet ef database update
> dotnet ef migrations remove

```
