import client from "./client/AxiosClient.js"
import 'dotenv/config'

const getOutcomesAndAssignmentsFromResults = async (results) => {
    let outcomes = {}
    let assignments = {}

    for (const result of results) {
        const outcomeId = result.links.learning_outcome
        let outcome = outcomes[outcomeId]

        if (outcome === undefined) {
            outcome = await getOutcome(outcomeId)
            outcomes[outcomeId] = outcome
        }

        const assignmentId = result.links.assignment.substring('assignment_'.length)
        let assignment = assignments[assignmentId]

        if (assignment === undefined) {
            assignment = await getAssignment(assignmentId)
            assignments[assignmentId] = assignment
        }

        if (outcome["assignments"] === undefined) {
            outcome["assignments"] = []
        }

        outcome["assignments"].push(assignment)

        if (assignment["outcomes"] === undefined) {
            assignment["outcomes"] = []
        }

        assignment["outcomes"].push(outcome)
    }

    return {
        outcomes: outcomes, assignments: assignments
    }
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

const getAssignment = async (id) => {
    process.stdout.write(`Fetching assignment ${id}... `)

    return client.get(`v1/courses/${courseId}/assignments/${id}`)
        .then((response) => {
            const data = response.data
            console.log('\x1b[32m%s\x1b[0m', `done ${data.name}`)

            return data
        })
}

const logOutcomes = (outcomes) => {
    Object.values(outcomes).forEach(outcome => {
        console.log(`Outcome: ${outcome.title}`)

        outcome.assignments.forEach(assignment => {
            console.log(`- ${assignment.name}`)
        })

        console.log('\n===========================================\n')
    })
}

const logAssignments = (assignments) => {
    Object.values(assignments).forEach(assignment => {
        console.log(`Assignment ${assignment.id}, link: ${assignment.html_url}`)
        console.log(`Name = ${assignment.name}\n`)

        assignment.outcomes.forEach(outcome => {
            console.log(`- ${outcome.title}`)
        })

        console.log('\n===========================================\n')
    })
}

// Get Canvas course id from the .env file
const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

// Fetch outcome results for the targeted course, this will return all the graded outcomes of the course
client.get(`v1/courses/${courseId}/outcome_results?per_page=500`)
    .then(async (response) => {
        const data = response.data
        const outcomeResults = data.outcome_results.filter(result => result.mastery)

        const result = await getOutcomesAndAssignmentsFromResults(outcomeResults)

        logOutcomes(result.outcomes)
        logAssignments(result.assignments)
    })