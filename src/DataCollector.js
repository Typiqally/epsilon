let outcomes = {}

export default {
    outcomes: outcomes,
    addModule: module => {
        outcomes[module.id] = module
        outcomes[module.id]["assignments"] = {}
    }, addAssignment: (module, assignment) => {
        outcomes[module.id]["assignments"][assignment.id] = assignment
    }
}