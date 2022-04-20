# Epsilon
Epsilon is an application for students from Fontys HBO-ICT education that are following open innovation. 
The application will gather all your mastered [KPI's](https://hbo-i.nl/domeinbeschrijving/) and export your KPI's to a file format, JSON, Exel, csv.

## How to use it

### 1. Clone the repository 
``git clone https://github.com/Typiqally/epsilon.git``

### 2. Copy .env.example
In the cloned project duplicate the file called ``.env.example`` and rename it to ``.env``

### 3. Canvas access token
Go to [https://fhict.instructure.com/profile/settings](https://fhict.instructure.com/profile/settings), navigate to 'Approved Interrogations' and create an new access token.\
Then past the access token in the ``.env`` file.\
for example ``CANVAS_TOKEN=WGWJKWLKWUYRBBL``

### 4. Personal course id 
With in canvas navigate to your personal course. The Id of your personal course can be found in the url.\
For example ``https://fhict.instructure.com/courses/00001/modules``, ``0001`` is in this case your personal course id.\
Past this id in the ``.env`` file. ``CANVAS_COURSE_ID=0001``

### 5. Install packages
Install the needed packages with npm ```npm i```

### 6. Run the application
fetch all data show gathered KPI's  ```npm index```