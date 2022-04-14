import client from "./client/AxiosClient.js"
import 'dotenv/config'

// Get Canvas course id from the .env file
const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

// Fetch outcome results for the targeted course, this will return all the graded outcomes of the course
client.get(`v1/courses/${courseId}/outcome_results?per_page=500`)
    .then(function (response) {
        console.log(response.data.outcome_results)
    })