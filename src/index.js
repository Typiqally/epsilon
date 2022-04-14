import client from "./client/AxiosClient.js"
import 'dotenv/config'

const getOutcomesFromResults = async (results) => {
    let outcomes = {}

    for (const result of results) {
        const outcomeId = result.links.learning_outcome
        const outcome = outcomes[outcomeId]

        if (outcome === undefined) {
            outcomes[outcomeId] = await getOutcome(outcomeId)
        }
    }

    return outcomes
}

const getOutcome = async (id) => {
    process.stdout.write(`Fetching outcome ${id}... `)

    return client.get(`v1/outcomes/${id}`)
        .then((response) => {
            const data = response.data
            console.log('\x1b[32m%s\x1b[0m', `done ${data.title}`)

            return data
        })
}

const getAssignmentsFromResults = async (results) => {
    let assignments = {}

    for (const result of results) {
        const assignmentId = result.links.assignment.substring('assignment_'.length)
        const assignment = assignments[assignmentId]

        if (assignment === undefined) {
            assignments[assignmentId] = await getAssignment(assignmentId)
        }
    }

    return assignments
}

const getAssignment = async (id) => {
    process.stdout.write(`Fetching assignment ${id}... `)

    return client.get(`v1/courses/${courseId}/assignments/${id}`)
        .then((response) => {
            const data = response.data
            console.log('\x1b[32m%s\x1b[0m', `done ${data.name}`)

            return data
        })
}

// Get Canvas course id from the .env file
const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

// Fetch outcome results for the targeted course, this will return all the graded outcomes of the course
client.get(`v1/courses/${courseId}/outcome_results?per_page=500`)
    .then(async (response) => {
        const data = response.data
        const outcomes = await getOutcomesFromResults(data.outcome_results)
        const assignments = await getAssignmentsFromResults(data.outcome_results)

        console.log(outcomes)
        console.log(assignments)
    })