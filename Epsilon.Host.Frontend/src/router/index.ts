import { createRouter, createWebHistory } from "vue-router"
import PerformanceDashboard from "@/views/PerformanceDashboard.vue"
import AuthorizeUser from "@/views/AuthorizeUser.vue"

const routes = [
    {
        path: "/",
        name: "PerformanceDashboard",
        component: PerformanceDashboard,
    },
    {
        path: "/auth",
        name: "Authorize",
        component: AuthorizeUser,
    },
]
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
})
export default router
