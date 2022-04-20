# Epsilon
Epsilon is an application for students from Fontys HBO-ICT education that are following open innovation.
These students usually have a personal course within Canvas (from Instructure), which contains all of their study submissions and acts as a portfolio of sorts.
During each semester, it is requested to take note of all KPI's which have been proven.
To aid in these efforts, this application will gather all your mastered/proven [KPI's](https://hbo-i.nl/domeinbeschrijving/) and export your KPI's to a file format (e.g., JSON, Exel, CSV).

![Application demo](https://i.imgur.com/b2w6PNl.gif)

## Setup

### 1. Cloning

First of all, clone the project using the following command: `git clone https://github.com/Typiqally/epsilon.git`

![GitHub cloning](https://i.imgur.com/wVNiZsk.png)

### 2. Duplicate environment file

In the project root directory, duplicate the file called `.env.example` and rename it to `.env`.
This file contains all the nescessary settings for the application to work.

### 3. Canvas access token

Go to [https://fhict.instructure.com/profile/settings](https://fhict.instructure.com/profile/settings), navigate to "Approved Integrations" and generate a new access token.
After that, copy the access token and paste it in the duplicated `.env` file, under the `CANVAS_TOKEN` setting.

![Canvas access token generation](https://i.imgur.com/0ukmuMF.png)

### 4. Personal course ID

First, navigate to your personal course in Canvas. The identifier of your personal course can be found in the URL.
For example, `https://fhict.instructure.com/courses/00001`, in this case `00001` is your personal course identifier.
Paste this identifier in the `.env` file, under the `CANVAS_COURSE_ID` setting.

![Canvas personal identifier URL](https://i.imgur.com/BkjDNtB.png)

### 5. Install packages

Install the required dependencies with NPM by executing the following command in the project root directory:  `npm install`

### 6. Run the application

To run the application, execute the following command: `npm run index`.
This will create an index of all your KPI's and output them in the desired format.

## License

Epsilon is licensed under the terms of GPL v3. See [LICENSE](LICENSE) for details.
