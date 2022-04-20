import client from "./client/AxiosClient.js"

export default {
    getOutcome: async (id) => {
        console.log('Fetching outcome \x1b[34m%s\x1b[0m...', id)

        return client.get(`v1/outcomes/${id}`)
            .then(response => {
                return response.data
            })
            .catch(response => console.log(response))
    }, getAssignment: async (courseId, id) => {
        console.log('Fetching assignment \x1b[34m%s\x1b[0m...', id)

        return client.get(`v1/courses/${courseId}/assignments/${id}`)
            .then(response => {
                return response.data
            })
    }, getMasteredOutcomes: async (courseId) => {
        return await client.get(`v1/courses/${courseId}/outcome_results?per_page=69420`)
            .then(async response => {
                return response.data.outcome_results.filter(result => result.mastery)
            })
    }, getModules: async (courseId) => {
        return await client.get(`v1/courses/${courseId}/modules`)
            .then(async response => {
                return response.data
            })
    }, getModuleItems: async (courseId, moduleId) => {
        return await client.get(`v1/courses/${courseId}/modules/${moduleId}/items`)
            .then(async response => {
                return response.data
            })
    }
}