<template>
    <TabGroup class="slider">
        <TabList>
            <Tab
                v-slot="{ selected }"
                :class="{ 'slider-item': selected, 'slider-item': !selected }"
                >Performance dashboard</Tab
            >
            <Tab class="slider-item">Competence document</Tab>
        </TabList>
    </TabGroup>
</template>

<script lang="ts" setup>
import { TabGroup, TabList, Tab, TabPanels, TabPanel } from "@headlessui/vue"
</script>

<style scoped>
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
