import client from "./client/AxiosClient.js"
import 'dotenv/config'

const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

client.get(`v1/courses/${courseId}/outcome_results?per_page=500`)
    .then(function (response) {
        console.log(response.data.outcome_results)
    })