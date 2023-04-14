<template>
  <button @click="openWindow">
    Click here to authorize
  </button>
</template>

<script setup lang="ts">
import {ref} from "vue";
import router from "../router";

const windowRef = ref()

function openWindow() {
    windowRef.value = window.open(
        "https://localhost:7084/auth/challenge",
        "",
        "width=600,height=400"
    );

    const timer = setInterval(() => {
        if (windowRef.value.closed) {
            router.push("/")
            clearInterval(timer)
        }
    }, 500);
}
</script>

<style scoped>
button {
    background: #535bf2;
}
</style>
