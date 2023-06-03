import { createRouter, createWebHistory } from "vue-router"
import AuthorizeUser from "@/views/AuthorizeUser.vue"
import Homepage from "@/views/Homepage.vue"

const routes = [
    {
        path: "/",
        name: "Homepage",
        component: Homepage,
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
