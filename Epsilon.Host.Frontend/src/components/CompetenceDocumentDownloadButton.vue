<template>
    <a @click="downloadCompetenceDocument()">
      <template v-if="!isDownloading">
        <span class="icon-wrapper">
          <font-awesome-icon icon="file-arrow-down" size="lg" alt="download-button-icon" />
        </span>
      </template>
      <template v-else>
        <RoundLoader class="loading-icon"/>
      </template>
      Download document
    </a>
  </template>
  
  <script lang="ts" setup>
    import { ref } from 'vue'
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
    import RoundLoader from "@/components/RoundLoader.vue"
    import axios from 'axios'

    const isDownloading = ref(false);
  
    async function downloadCompetenceDocument() {
        isDownloading.value = true;
        axios({
            method: 'get',
            url: 'https://localhost:7084/document/word',
            responseType: 'arraybuffer',
        })
            .then((response) => {
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'competence_document.docx');
                document.body.appendChild(link);
                link.click();
            })
                .catch(() => console.log('error occurred'))
                .finally(() => {
                    isDownloading.value = false; // Update isDownloading to false after the download is completed or failed
            });
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
