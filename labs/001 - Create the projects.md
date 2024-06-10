# Lab: Starting a New Project with ASP.NET and Class Library

In this lab, you will create an empty solution file named "Library" in the root directory and then add two projects to a "src" directory: an ASP.NET project named "Library.Web" and a class library named "Library".

## Prerequisites

- .NET 8 SDK installed on your machine
- Visual Studio 2022 or Visual Studio Code installed

## Steps

### 1. Create an Empty Solution

#### Using Command Line/VS Code

1. Open a terminal.
2. Navigate to the directory where you want to create your solution.
3. Run the following command to create an empty solution named `Library`:

    ```bash
    dotnet new sln -n Library
    ```

#### Using Visual Studio

1. Open Visual Studio.
2. Select **File** > **New** > **Project**.
3. In the **Create a new project** dialog, select **Blank Solution**.
4. Click **Next**.
5. Name the solution `Library`.
6. Choose a location for your solution and click **Create**.

### 2. Add an ASP.NET Project

#### Using Command Line/VS Code

1. Create a `src` directory if it doesn't exist:

    ```bash
    mkdir src
    ```

2. Run the following command to create an ASP.NET project named `Library.Web` inside the `src` directory:

    ```bash
    dotnet new webapp -n Library.Web -o src/Library.Web
    ```

3. Add the `Library.Web` project to the solution:

    ```bash
    dotnet sln Library.sln add src/Library.Web/Library.Web.csproj
    ```

#### Using Visual Studio

1. Create a `src` directory in your solution folder.
2. Right-click on the **Library** solution in **Solution Explorer**.
3. Select **Add** > **New Project**.
4. In the **Add a new project** dialog, select **ASP.NET Core Web App**.
5. Click **Next**.
6. Name the project `Library.Web`.
7. Set the location to the `src` directory inside your solution folder.
8. Click **Create**.
9. Select **.NET 8.0 (Standard Term Support)** and click **Create**.

### 3. Add a Class Library Project

#### Using Command Line/VS Code

1. Run the following command to create a class library project named `Library` inside the `src` directory:

    ```bash
    dotnet new classlib -n Library -o src/Library
    ```

2. Add the `Library` project to the solution:

    ```bash
    dotnet sln Library.sln add src/Library/Library.csproj
    ```

#### Using Visual Studio

1. Right-click on the **Library** solution in **Solution Explorer**.
2. Select **Add** > **New Project**.
3. In the **Add a new project** dialog, select **Class Library**.
4. Click **Next**.
5. Name the project `Library`.
6. Set the location to the `src` directory inside your solution folder.
7. Click **Create**.
8. Select **.NET 8.0 (Standard Term Support)** and click **Create**.

### 4. Adding a Reference to the Library Project from the Web Project

#### Using Command Line/VS Code

1. Run the following command to add a reference to the `Library` project from the `Library.Web` project:

    ```bash
    dotnet add src/Library.Web/Library.Web.csproj reference src/Library/Library.csproj
    ```

#### Using Visual Studio

1. Right-click on the **Library.Web** project in **Solution Explorer**.
2. Select **Add** > **Project Reference**.
3. In the **Reference Manager** dialog, check the box next to the **Library** project.
4. Click **OK**.

### 5. Verify the Solution Structure

After completing the above steps, your solution structure should look like this:

```
Library.sln
|-- src
|   |-- Library.Web
|   |   |-- Library.Web.csproj
|   |   |-- ...
|   |
|   |-- Library
|       |-- Library.csproj
|       |-- ...
```

### Conclusion

You have successfully created an empty solution and added an ASP.NET project and a class library project within the `src` directory.