<template>
    <a class="download-btn" @click="downloadCompetenceDocument()">
        <template v-if="!isDownloading">
            <span class="icon-wrapper">
                <font-awesome-icon
                    icon="file-arrow-down"
                    size="lg"
                    alt="download-button-icon" />
            </span>
        </template>
        <template v-else>
            <RoundLoader class="loading-icon" />
        </template>
        Download document
    </a>
</template>

<script lang="ts" setup>
import { ref } from "vue"
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome"
import RoundLoader from "@/components/RoundLoader.vue"
import { Api } from "../logic/Api"

const api = new Api()

const isDownloading = ref(false)

async function downloadCompetenceDocument(): Promise<void> {
    // Setting downloading ref to true, triggering loading icon
    isDownloading.value = true

    const wordlist = await api.document.wordList()
    const data = await wordlist.text()
    const blob = new Blob([data], { type: "application/octet-stream" })

    // Create a temporary anchor element
    const downloadLink = document.createElement("a")
    downloadLink.href = URL.createObjectURL(blob)
    downloadLink.download = "competence_document.docx"

    // Programmatically trigger the download
    downloadLink.click()

    // Clean up resources
    URL.revokeObjectURL(downloadLink.href)

    // Setting downloading ref to false, triggering regular icon
    isDownloading.value = false
}
</script>

<style lang="scss" scoped>
$icon-size: 40px;

.download-btn {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: fit-content;
    height: 50px;
    padding: 0.6rem 1.2rem;
    color: black;
    background-color: #f2f3f8;
    font-weight: 400;
    text-align: center;
    border-radius: 8px;
    cursor: pointer;

    &:hover {
        color: white;
        background-color: #848da4;
    }
}

.icon-wrapper {
    margin-right: 1rem;
}

.loading-icon {
    width: $icon-size;
    height: $icon-size;
    margin-right: 1rem;
}
</style>
