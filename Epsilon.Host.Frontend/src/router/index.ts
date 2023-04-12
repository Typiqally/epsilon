import {createRouter, createWebHistory} from 'vue-router'
import PerformanceDashboard from "@/views/PerformanceDashboard.vue";

const routes = [
    {
        path: '/',
        name: 'PerformanceDashboard',
        component: PerformanceDashboard
    }
]
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
})
export default router
