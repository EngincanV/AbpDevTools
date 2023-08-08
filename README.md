# AbpDevTools
A set of tools to make development with ABP easier. It's a dotnet tool and accessed via `abpdev` **CLI** command.

It helps the developer, build, run, replace, and manage logs of the projects. It makes running **multiple** solutions and projects easier.

# Installation

```bash
dotnet tool install -g AbpDevTools
```

## Local Installation
If you don't have access to the package source. You can install it from the source code by the following code:

```bash
pwsh install.ps1
```

# Usage

Execute `abpdev` command in the terminal and it'll show you the help message.

```bash
abpdev --help
```

# Commands
The following commands are available:

## abpdev build
Builds all solutions/projects in the current directory recursively. _(Multiple solutions)_

```
abpdev build <workingdirectory> [options]
```

```bash
abpdev build -h

PARAMETERS
  workingdirectory  Working directory to run build. Probably project or solution directory path goes here. Default: . (Current Directory) 
OPTIONS
  -f|--build-files  (Array) Names or part of names of projects or solutions will be built.
  -i|--interactive  Interactive build file selection. Default: "False".
  -c|--configuration
  -h|--help         Shows help text.
```

Convention: `*.sln` files are considered as solutions and `*.csproj` files are considered as projects.

![abpdev build](images/abpdevbuild.gif)

### Example commands

- Run in a specific path
    ```bash
    abpdev build C:\Path\To\Projects
    ```

- Run in a specific configuration
    ```bash
    abpdev build C:\Path\To\Projects -c Release
    ```


- Run in a specific path with specific configuration and specific projects
    ```bash
    abpdev build C:\Path\To\Projects -c Release -f ProjectA.csproj ProjectB.csproj
    ```

- Run in interactive mode **(Select projects to build)**
    ```bash
    abpdev build -i
    ```
    ![abpdev build interactive](images/abpdevbuild-interactive.gif)

## abpdev run
Runs the solution in the current directory. _(Multiple solution, multiple applications including DbMigrator)_

```
abpdev run <workingdirectory> [options]
```

```bash
PARAMETERS
  workingdirectory  Working directory to run build. Probably project or solution directory path goes here. Default: . (Current Directory)

OPTIONS
  -w|--watch        Watch mode Default: "False".
  --skip-migrate    Skips migration and runs projects directly. Default: "False".
  -a|--all          Projects to run will not be asked as prompt. All of them will run. Default: "False".
  --no-build        Skipts build before running. Passes '--no-build' parameter to dotnet run. Default: "False".
  -i|--install-libs  Runs 'abp install-libs' command while running the project simultaneously. Default: "False".
  -g|--graphBuild   Uses /graphBuild while running the applications. So no need building before running. But it may cause some performance. Default: "False".
  -p|--projects     (Array) Names or part of names of projects will be ran.
  -c|--configuration
  -e| --env        Virtual Environment name. You can manage virtual environments by using 'abpdev env config'
  -h|--help         Shows help text.
```

Convention: `*.csproj` files with specific names are considered as applications or dbmigrators.

> _Use `abpdev run config` command to change project name conventions according to your requirements_

![abpdev run](images/abpdevrun.gif)

### Example commands

- Run multiple solutions
    ```bash
    abpdev run C:\Path\To\Top\Folder\Of\Solutions
    ```
    ![abpdev run multiple solutions](images/abpdevrun-multiplesolutions.gif)

- Run in a specific path
    ```bash
    abpdev run C:\Path\To\Projects
    ```

- Run in a specific configuration and specific path
    ```bash
    abpdev run C:\Path\To\Projects -c Release
    ```

- Run all projects instead prompt selection
    ```bash
    abpdev run -a
    ```

    ![abpdev run all](images/abpdevrun-all.gif)

- Skip migration and run projects directly
    ```bash
    abpdev run --skip-migrate
    ```

- Run in watch mode
    ```bash
    abpdev run -w
    ```

## Virtual Environments
Virtual environments are used to run multiple solutions with different configurations. For example, you can run different solutions with different environments _(connectionstrings etc.)_.

```bash
abpdev env config
```

> You'll see the following screen. You can add, edit, delete, and select virtual environments.
> ```json
> {
>  "SqlServer": {
>    "Variables": {
>      "ConnectionStrings__Default": "Server=localhost;Database={AppName}_{Today};User ID=SA;>Password=12345678Aa;TrustServerCertificate=True"
>    }
>  },
>  "MongoDB": {
>    "Variables": {
>      "ConnectionStrings__Default": "mongodb://localhost:27017/{AppName}_{Today}"
>    }
>  }
>}
> ```
> **{Today}** will be replaced with the current date. So you can run multiple solutions with different databases.
> **{AppName}** will be replaced with the application name. So you can run multiple solutions with different databases. _When app name couldn't be detected, folder name will be used.


### Example commands

- Run in a specific virtual environment
    ```bash
    abpdev run -e SqlServer
    ```


## abpdev logs
Finds given project under the current directory and shows logs of it.

![abpdev logs](images/abpdevlogs.gif)
---
![abpdev logs clear](images/abpdevlogs-clear.gif)


```bash
  abpdev logs <projectname> [options]
  abpdev logs [command] [...]

PARAMETERS
  projectname       Determines the project to open logs of it.

OPTIONS
  -p|--path         Working directory of the command. Probably solution directory. Default: . (CurrentDirectory)
  -i|--interactive  Options will be asked as prompt when this option used. Default: "False".
  -h|--help         Shows help text.

COMMANDS
  clear
```

### Example commands

- Show logs of the **.Web** project
    ```bash
    abpdev logs Web
    ```

- Clear logs of the **.Web** project
    ```bash
    abpdev logs clear -p Web
    ```

- Clear logs without approval
    ```bash
    abpdev logs clear -p Web -f
    ```

## abpdev replace
Replaces specified text in files under the current directory recursively. Mostly used to replace connection strings in `appsettings.json` files. But it can be used for any other purposes. 

```bash
USAGE
  abpdev replace <replacementconfigname> [options]
  abpdev replace [command] [...]

DESCRIPTION
  Runs file replacement according to configuration.

PARAMETERS
  replacementconfigname  If you execute single option from config, you can pass the name or pass 'all' to execute all of them

OPTIONS
  -p|--path         Working directory of the command. Probably solution directory. Default: . (CurrentDirectory)
  -i|--interactive  Interactive Mode. It'll ask prompt to pick one config. Default: "False".
  -h|--help         Shows help text.

COMMANDS
  config            Allows managing replacement configuration. Subcommands: config clear.
```

![abpdev replace](images/abpdevreplace.gif)


> Use `abpdev replace config` command to change file name conventions according to your requirements.
> _You'll see something like that by default:
> ```json 
> {
>   "ConnectionStrings": {
>     "FilePattern": "appsettings.json",
>     "Find": "Trusted_Connection=True;",
>     "Replace": "User ID=SA;Password=12345678Aa;"
>   }
> }
> ```

### Example commands

- Replace connection strings in `appsettings.json` files
    ```bash
    abpdev replace ConnectionStrings
    ```

- Run all the configurations at once
    ```bash
    abpdev replace all
    ```

## Enable Notifications
You can enable notifications to get notified when a build or run process is completed. You can enable it by using `abpdev enable-notifications` command and disable it by using `abpdev disable-notifications` command.

> _It only works on **Windows** and **MacOS**. Linux is not supported yet._

```bash
abpdev enable-notifications
abpdev disable-notifications
```

It'll send a notification when a **migration**, **build** or **run** process is completed.

![abpdev notifications](images/abpdevnotification.gif)


## Environment Apps
You can easily run commonly used environment apps like **SQL Server**, **PostgreSQL**, **Redis**, **MySQL**, **MongoDB** and **RabbitMQ** by using `abpdev envapp start` command.

```bash
abpdev envapp [command] <appname> [options]

abpdev envapp start <appname> [options]
abpdev envapp stop <appname> [options]
```

> You can change the default running commands by using `abpdev envapp config` command.

![abpdev envapp](images/abpdevenvapp.gif)

Available app names by **default**:
```bash
 - sqlserver
 - sqlserver-edge
 - postgresql
 - mysql
 - mongodb
 - redis
 - rabbitmq
```

_You can extend the list or change environments of apps by using `abpdev envapp config` command._

### Example commands

- Start SQL Server with custom SA password
    ```bash
    abpdev envapp start sqlserver -p myPassw0rd
    ```
