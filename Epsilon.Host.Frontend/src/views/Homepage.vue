<template class="homepage">
    <div class="banner">
        <img
            src="../assets/Epsilon_Logo_Blue-Blue.png"
            alt="logo"
            class="logo" />
        <div class="selection-boxes">
            <DropdownBtn
                v-if="selectedTerm"
                v-model="selectedTerm"
                :items="terms" />
        </div>
    </div>
    <TabGroup as="template">
        <div class="slider">
            <TabList>
                <Tab class="slider-item">Performance dashboard</Tab>
                <Tab class="slider-item">Competence document</Tab>
            </TabList>
        </div>
        <TabPanels>
            <TabPanel>
                <PerformanceDashboard :till-date="getCorrectedTermDate" />
            </TabPanel>
            <TabPanel>
                <div>
                    <h1 align="left">Persona</h1>
                    <br />
                    <QuillEditor theme="snow" @ready="onEditorReady($event)" />
                </div>
            </TabPanel>
        </TabPanels>
    </TabGroup>
</template>

<script lang="ts" setup>
import {
    Api,
    CompetenceProfile,
    EnrollmentTerm,
    HttpResponse,
} from "../logic/Api"
import PerformanceDashboard from "./PerformanceDashboard.vue"
import DropdownBtn from "@/components/DropdownBtn.vue"
import { QuillEditor } from "@vueup/vue-quill"
import axios from "axios"
import "@vueup/vue-quill/dist/vue-quill.snow.css"
import { onMounted, Ref, ref } from "vue"
import { TabGroup, TabList, Tab, TabPanels, TabPanel } from "@headlessui/vue"
import { computed } from "vue"

const data: Ref<CompetenceProfile | undefined> = ref(undefined)

const terms: Ref<EnrollmentTerm[]> = ref([])
const selectedTerm: Ref<EnrollmentTerm | undefined> = ref(undefined)

const personaHtml = ref("")

const App = new Api()

const onEditorReady = (quill: any) => {
    quill.root.innerHTML = personaHtml.value
}

onMounted(async () => {
    try {
        const response = await axios.get(
            "https://localhost:7084/component/persona_page"
        )
        personaHtml.value = response.data.personaHtml
    } catch (error) {
        console.error("Failed to retrieve persona page HTML:", error)
    }

    App.filter
        .participatedTermsList()
        .then((r: HttpResponse<EnrollmentTerm[]>) => {
            terms.value = r.data
            selectedTerm.value = terms.value[0]
        })

    App.component
        .componentDetail("competence_profile", {
            startDate: "02-26-2023",
            endDate: "05-26-2023",
        })
        .then((r: HttpResponse<CompetenceProfile>) => {
            data.value = r.data
        })
})

const getCorrectedTermDate = computed(() => {
    if (!selectedTerm.value) {
        return undefined
    }

    const index = terms.value?.indexOf(selectedTerm.value) as number
    if (index > 0) {
        return terms.value?.at(index - 1)?.start_at
    } else {
        return terms.value?.at(index)?.end_at
    }
})
</script>

<style scoped>
.homepage {
    width: 1366px;
}
.banner {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;

    background-color: #f2f3f8;
    height: 9rem;
    width: 100%;
    max-width: 1366px;
    border-radius: 0.5rem;
}
.logo {
    height: 5rem;
    margin-left: 2rem;
}

.selection-boxes {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
}

.dropdown:focus,
.dropdown:active {
    outline: transparent;
}

.slider {
    list-style: none;
    background-color: #f2f3f8;
    margin: 2rem 0;
    padding: 5px;
    border-radius: 0.5rem;
    min-height: 40px;
    width: fit-content;
    display: flex;
    align-items: center;
    justify-content: center;
}

.slider-item {
    color: black;
    border-radius: 0.4rem;
    cursor: pointer;
    background-color: transparent;
    border: none;
    position: relative;
    z-index: 2;
}

.slider-item:active,
.slider-item:focus {
    outline: transparent;
}

.slider-item:hover {
    background-color: #d8d9dd;
}

.slider-item[data-headlessui-state="selected"] {
    background-color: white;
}
</style>
