let DataCollector = class {
    fullListOfOutcomes = {}

    getList() {
        return this.fullListOfOutcomes;
    }

    addModule(module) {
        this.fullListOfOutcomes[module.id] = module
        this.fullListOfOutcomes[module.id]["assignments"] = {}
    }

    addAssignment(module, assignment) {
        this.fullListOfOutcomes[module.id]["assignments"][assignment.id] = assignment
    }
}

export default DataCollector