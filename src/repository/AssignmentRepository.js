import client from "./client/AxiosClient.js"

export default {
    get: async (courseId, id) => {
        console.log('Fetching assignment \x1b[34m%s\x1b[0m...', id)

        return client.get(`v1/courses/${courseId}/assignments/${id}`)
            .then(response => {
                return response.data
            })
    }
}