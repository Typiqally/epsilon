import client from "./client/AxiosClient.js"

export default {
    get: async (courseId) => {
        return await client.get(`v1/courses/${courseId}/modules`)
            .then(async response => {
                return response.data
            })
    }, getItems: async (courseId, moduleId) => {
        return await client.get(`v1/courses/${courseId}/modules/${moduleId}/items`)
            .then(async response => {
                return response.data
            })
    }
}