import client from "./client/AxiosClient.js"
import 'dotenv/config'
import DataCollector from "./DataCollector.js";
import CanvasApi from "./CanvasApi.js";

const groupOutcomesWithAssignment = async (results) => {
    let GroupedByAssignment = {}

    for (const result of results) {
        if (result.links.assignment !== undefined) {
            const assignmentId = result.links.assignment.substring('assignment_'.length)
            if (GroupedByAssignment[assignmentId.toString()] === undefined) {
                GroupedByAssignment[assignmentId.toString()] = []
            }
            GroupedByAssignment[assignmentId.toString()].push(result)
        }
    }

    return GroupedByAssignment
}

const mergeDataSources = async () => {
    for (const module of modules) {
        let moduleItems = await canvasApi.getModuleItems(courseId, module.id);
        dataCollector.addModule(module)
        for (const item of moduleItems) {
            //Is the item an assignment
            if (item.type === "Assignment") {
                let assignment = await canvasApi.getAssignment(courseId, item.content_id)
                //Has the assignment results that are mastered
                if (masteredOutcomes[assignment.id] !== undefined) {
                    assignment.results = masteredOutcomes[assignment.id]
                    assignment.results.forEach((async (outcome, index) => {
                        assignment.results[index].outcome = await canvasApi.getOutcome(outcome.links.learning_outcome);
                    }))
                    dataCollector.addAssignment(module, assignment)
                }
            }
        }
    }
}

const logResults = () => {
    Object.values(dataCollector.getList()).forEach((Module => {
        console.log('Module', Module.name)
        if (Object.keys(Module.assignments).length > 0) {
            Object.values(Module.assignments).forEach(assignment => {
                console.log('\t', assignment.name)
                Object.values(assignment.results).forEach(item => {
                    console.log('\t\t', item.outcome.title)
                })
            });
        } else {
            console.log('\tNo assignments could be found')
        }
        console.log('\n===========================================\n')
    }))
}

// Get Canvas course id from the .env file
const courseId = process.env.CANVAS_COURSE_ID
console.log(`Targeting course: ${courseId}`)

let dataCollector = new DataCollector()
let canvasApi = new CanvasApi()
//Get modules form course
let modules = await canvasApi.getModules(courseId);

// fetch all mastered outcomes and group them on assigment.
let masteredOutcomes = await groupOutcomesWithAssignment(await canvasApi.getMasteredOutcomes(courseId));

await mergeDataSources()
//Write gathered data
logResults()