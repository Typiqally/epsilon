import { createApp } from "vue"
import router from "./router"
import "./style.scss"

import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome"
import { library } from "@fortawesome/fontawesome-svg-core"
import { faFileArrowDown } from "@fortawesome/free-solid-svg-icons"
import App from "./App.vue"

library.add(faFileArrowDown)

const app = createApp(App).use(router).mount("#app")
app.component("FontAwesomeIcon", FontAwesomeIcon)
app.mount("#app")
