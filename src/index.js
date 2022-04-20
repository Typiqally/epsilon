import 'dotenv/config'
import DataCollector from "./DataCollector.js";
import CanvasApi from "./CanvasApi.js";

const groupOutcomesWithAssignment = async outcomes => {
    let assignments = {}

    for (const outcome of outcomes) {
        const assignmentId = outcome.links.assignment.substring('assignment_'.length)

        if (assignments[assignmentId] === undefined) {
            assignments[assignmentId] = []
        }

        assignments[assignmentId].push(outcome)
    }

    return assignments
}

const mergeDataSources = async (modules, outcomes) => {
    for (const module of modules) {
        const items = await CanvasApi.getModuleItems(courseId, module.id);
        const assignmentItem = items.filter(item => item.type === "Assignment");

        dataCollector.addModule(module)

        for (const item of assignmentItem) {
            //Is the item an assignment
            const assignment = await CanvasApi.getAssignment(courseId, item.content_id)

            //Has the assignment results that are mastered
            if (outcomes[assignment.id] !== undefined) {
                assignment.results = outcomes[assignment.id]
                assignment.results.forEach((async (outcome, index) => {
                    assignment.results[index].outcome = await CanvasApi.getOutcome(outcome.links.learning_outcome);
                }))

                dataCollector.addAssignment(module, assignment)
            }
        }
    }
}

const logResults = () => {
    Object.values(dataCollector.getList()).forEach(module => {
        console.log('Module', module.name)
        if (Object.keys(module.assignments).length > 0) {
            Object.values(module.assignments).forEach(assignment => {
                console.log('\t', assignment.name)
                Object.values(assignment.results).forEach(item => {
                    console.log('\t\t', item.outcome.title)
                })
            });
        } else {
            console.log('\tNo assignments could be found')
        }

        console.log('\n===========================================\n')
    })
}

// Get Canvas course id from the .env file
const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

let dataCollector = new DataCollector()

const modules = await CanvasApi.getModules(courseId);
const outcomes = await CanvasApi.getMasteredOutcomes(courseId);
const outcomesWithAssignments = await groupOutcomesWithAssignment(outcomes);

await mergeDataSources(modules, outcomesWithAssignments)

logResults()