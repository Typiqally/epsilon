# Epsilon
Epsilon is an application for students from Fontys HBO-ICT education that are following open innovation.
These students usually have a personal course within Canvas (from Instructure), which contains all of their study submissions and acts as a portfolio of sorts.
During each semester, it is requested to take note of all KPI's which have been proven.
To aid in these efforts, this application will gather all your mastered/proven [KPI's](https://hbo-i.nl/domeinbeschrijving/) and export your KPI's to a file format (e.g., JSON, Exel, CSV).

![Application demo](https://i.imgur.com/xe9C939.gif)

## Development

### 1. Cloning

First of all, clone the project using the following command: `git clone https://github.com/Typiqally/epsilon.git`

![GitHub cloning](https://i.imgur.com/wVNiZsk.png)

### 2. Duplicate environment file

In the project root directory, duplicate the file called `appsettings.example.json` and rename it to `appsettings.json`.
This file contains all the necessary settings for the application to work.

### 3. Canvas access token

Go to [https://fhict.instructure.com/profile/settings](https://fhict.instructure.com/profile/settings), navigate to "Approved Integrations" and generate a new access token.

![Canvas access token generation](https://i.imgur.com/0ukmuMF.png)

After that, copy the access token and paste it in the `appsettings.json` file, under the `Canvas.AccessToken` setting.

### 4. Personal course ID

First, navigate to your personal course in Canvas. The identifier of your personal course can be found in the URL.
For example, `https://fhict.instructure.com/courses/00001`, in this case `00001` is your personal course identifier.

![Canvas personal identifier URL](https://i.imgur.com/BkjDNtB.png)

Paste this identifier in the `appsettings.json` file, under the `Canvas.CourseId` setting.

### 5. Run the application

To run the application, execute the following command: `dotnet run`.
This will create an index of all your KPI's and output them in the desired format.

## License

Epsilon is licensed under the terms of GPL v3. See [LICENSE](LICENSE) for details.
