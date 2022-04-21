import client from "./client/AxiosClient.js"

export default {
    get: async (id) => {
        console.log('Fetching outcome \x1b[34m%s\x1b[0m...', id)

        return client.get(`v1/outcomes/${id}`)
            .then(response => {
                return response.data
            })
            .catch(response => console.log(response))
    }, getMasteredResults: async (courseId) => {
        return await client.get(`v1/courses/${courseId}/outcome_results?per_page=69420`)
            .then(async response => {
                return response.data.outcome_results.filter(result => result.mastery)
            })
    }
}