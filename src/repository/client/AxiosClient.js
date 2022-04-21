import axios from 'axios'
import 'dotenv/config'

const baseUrl = process.env.CANVAS_API_BASE_URL
const token = process.env.CANVAS_TOKEN

axios.defaults.baseURL = baseUrl
axios.defaults.headers.common = {'Authorization': `Bearer ${token}`}

export default axios