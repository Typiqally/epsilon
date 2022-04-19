

let DataCollector = class {
    fullListOfOutcomes = {};

    getList(){
        return this.fullListOfOutcomes;
    }

    addModule(module){
        this.fullListOfOutcomes[module.id.toString()] = module
        this.fullListOfOutcomes[module.id.toString()]["assignments"] = {}
    }

    addAssignment(module, assignment){
        this.fullListOfOutcomes[module.id.toString()]["assignments"][assignment.id.toString()] = assignment
    }
}

export default DataCollector