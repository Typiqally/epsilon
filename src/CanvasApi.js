import client from "./client/AxiosClient.js"

let CanvasApi = class {
    client = client

    getOutcome = async (id) => {
        process.stdout.write(`Fetching outcome ${id}... `)

        return this.client.get(`v1/outcomes/${id}`)
            .then((response) => {
                const data = response.data
                // console.log('\x1b[32m%s\x1b[0m', `done ${data.title}`)

                return data
            }).catch((response => console.log(response)))
    }

    getAssignment = async (courseId,id) => {
        process.stdout.write(`Fetching assignment ${id}... `)

        return this.client.get(`v1/courses/${courseId}/assignments/${id}`)
            .then((response) => {
                const data = response.data
                // console.log('\x1b[32m%s\x1b[0m', `done ${data.name}`)

                return data
            })
    }

    getMasteredOutcomes = async (courseId) => {
        return await client.get(`v1/courses/${courseId}/outcome_results?per_page=69420`)
            .then(async (response) => {
                return  response.data.outcome_results.filter(result => result.mastery)
            })
    }

    getModules = async (courseId) => {
        return await client.get(`v1/courses/${courseId}/modules`)
            .then(async (response) => {
                return response.data
            })
    }

    getModuleItems = async (courseId, moduleId) => {
        return await client.get(`v1/courses/${courseId}/modules/${moduleId}/items`)
            .then(async (response) => {
                return response.data
            })
    }
}

export default CanvasApi