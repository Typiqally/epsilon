<template class="homepage">
    <div class="banner">
        <img
            src="../assets/Epsilon_Logo_Blue-Blue.png"
            alt="logo"
            class="banner-logo" />
        <div class="selection-boxes">
            <div v-if="selectedStudent" class="student-select">
                <img
                    :src="selectedStudent.avatarUrl"
                    :alt="selectedStudent.name"
                    class="student-select-image" />
                <SearchBox v-model="selectedStudent" :items="students" />
            </div>
            <ListBox
                v-if="selectedTerm"
                v-model="selectedTerm"
                :items="terms" />
        </div>
    </div>
    <TabGroup as="template">
        <div class="tabs">
            <div class="slider">
                <TabList>
                    <Tab class="slider-item">Performance dashboard</Tab>
                    <Tab class="slider-item">Competence document</Tab>
                </TabList>
            </div>
            <div>
                <CompetenceDocumentDownloadButton />
            </div>
        </div>
        <hr class="tab-border" />
        <TabPanels>
            <TabPanel>
                <PerformanceDashboard :till-date="getCorrectedTermDate" />
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
    User,
} from "../logic/Api"

import PerformanceDashboard from "./PerformanceDashboard.vue"
import ListBox from "@/components/ListBox.vue"
import SearchBox from "@/components/SearchBox.vue"
import { onMounted, Ref, ref } from "vue"
import { TabGroup, TabList, Tab, TabPanels, TabPanel } from "@headlessui/vue"
import { computed } from "vue"
import CompetenceDocumentDownloadButton from "@/components/CompetenceDocumentDownloadButton.vue"

const data: Ref<CompetenceProfile | undefined> = ref(undefined)

const students: Ref<User[]> = ref([])
const selectedStudent: Ref<User | undefined> = ref(undefined)

const terms: Ref<EnrollmentTerm[]> = ref([])
const selectedTerm: Ref<EnrollmentTerm | undefined> = ref(undefined)

const App = new Api()

onMounted(() => {
    App.filter
        .participatedTermsList()
        .then((r: HttpResponse<EnrollmentTerm[]>) => {
            terms.value = r.data
            selectedTerm.value = terms.value[0]
        })

    App.filter.accessibleStudentsList().then((r: HttpResponse<User[]>) => {
        students.value = r.data
        selectedStudent.value = students.value[0]
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

<style scoped lang="scss">
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

    &-logo {
        height: 5rem;
        margin-left: 2rem;
    }
}

.selection-boxes {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
}

.student-select {
    display: flex;

    &-image {
        width: 3rem;
        height: 3rem;
        aspect-ratio: 1/1;
        border: none;
        border-radius: 3rem;
        margin-right: 0.75rem;
        overflow: hidden;
        background-color: #fff;
    }
}

.tabs {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.slider {
    list-style: none;
    background-color: #f2f3f8;
    margin: 2rem 0;
    padding: 5px;
    border-radius: 8px;
    min-height: 40px;
    width: fit-content;
    display: flex;
    align-items: center;
    justify-content: center;

    &-item {
        color: black;
        border-radius: 5px;
        cursor: pointer;
        background-color: transparent;
        border: none;
        position: relative;
        z-index: 2;

        &:active,
        &:focus {
            outline: transparent;
        }

        &:hover {
            background-color: #d8d9dd;
        }

        &[data-headlessui-state="selected"] {
            background-color: white;
        }
    }
}

.tab-border {
    border: 1px solid #f2f3f8;
    border-right: none;
    //border-bottom: none;
    border-left: none;
    margin-bottom: 2.5rem;
}
</style>
