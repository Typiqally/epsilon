<template>
    <a class="download-btn" @click="downloadCompetenceDocument">
        <span class="icon-wrapper">
            <ArrowDownTrayIcon v-if="!isDownloading" />
            <RoundLoader v-else />
        </span>
        Download document
    </a>
</template>

<script lang="ts" setup>
import { ref } from "vue"
import RoundLoader from "@/components/RoundLoader.vue"
import { Api } from "../logic/Api"
import { ArrowDownTrayIcon } from "@heroicons/vue/20/solid"

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
    width: 24px;
    height: 24px;
}
</style>
