<template>
    <Listbox
        v-if="modelValue"
        :model-value="props.modelValue"
        @update:model-value="$emit('update:modelValue', $event)">
        <ListboxButton class="termselect dropdown">
            <span> {{ modelValue.name }}</span>
            <span class="list-arrow">
                <ChevronUpDownIcon aria-hidden="true" />
            </span>
        </ListboxButton>
        <ListboxOptions class="dropdown-options">
            <ListboxOption
                v-for="item in items"
                v-slot="{ active, selected }"
                :key="item.name"
                class="dropdown-option"
                :value="item"
                as="template">
                <li>
                    <span>{{ item.name }}</span>
                    <span v-if="selected" class="list-select">
                        <CheckIcon aria-hidden="true" />
                    </span>
                </li>
            </ListboxOption>
        </ListboxOptions>
    </Listbox>
</template>

<script lang="ts" setup>
import { defineProps } from "vue"
import {
    Listbox,
    ListboxButton,
    ListboxOptions,
    ListboxOption,
} from "@headlessui/vue"
import { ChevronUpDownIcon, CheckIcon } from "@heroicons/vue/20/solid"

const props = defineProps<{
    items: Array<{ name: string }>
    modelValue: { name: string }
}>()

defineEmits(["update:modelValue"])
</script>

<style scoped>
.profileselect {
    margin-right: 4rem;
    min-width: 15rem;
}

.termselect {
    min-width: 9rem;
}

.list-arrow svg,
.list-select svg {
    max-height: 30px;
    float: right;
}

.dropdown {
    border: none;
    font-weight: 400;
    position: relative;
    z-index: 1;
    text-align: left;
}

.dropdown:hover,
.dropdown:focus,
.dropdown:active {
    border: none;
    outline: none;
}

.dropdown-options {
    list-style-type: none;
    position: absolute;
    background-color: #fff;
    padding: 1rem;
    margin-top: 3rem;
    width: 10rem;
    border-radius: 6px;
    border: 1px solid #d8d8d8;
    text-align: left;
    z-index: 999;
}

.profileselect-options {
    margin-top: 3rem;
    margin-right: 12rem;
}

.dropdown-option {
    padding: 15px;
    cursor: pointer;
}
</style>
