import axios from 'axios'
import 'dotenv/config'

const token = process.env.CANVAS_TOKEN

axios.defaults.baseURL = 'https://fhict.instructure.com/api/'
axios.defaults.headers.common = {'Authorization': `Bearer ${token}`}

export default axios;