<template>
    <a @click="downloadCompetenceDocument()">
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

async function downloadCompetenceDocument() {
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

a {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 200px;
    color: black;
    background-color: #f2f3f8;
    text-align: center;
    padding: 1rem;
    border-radius: 9px;
}

a:hover {
    color: white;
    background-color: #848da4;
}

.icon-wrapper {
    width: $icon-size;
    height: $icon-size;
    display: flex;
    align-items: center;
    justify-content: center;
}

.loading-icon {
    width: $icon-size;
    height: $icon-size;
}
</style>
