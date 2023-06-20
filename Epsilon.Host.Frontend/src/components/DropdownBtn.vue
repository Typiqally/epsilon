<template>
    <Listbox v-model="selectedTerm">
        <ListboxButton class="termselect dropdown">
            <span> {{ selectedTerm.name }}</span>
            <span class="list-arrow">
                <ChevronUpDownIcon aria-hidden="true" />
            </span>
        </ListboxButton>
        <ListboxOptions class="dropdown-options">
            <ListboxOption
                v-slot="{ active, selected }"
                :key="term.name"
                v-for="term in terms"
                class="dropdown-option"
                :value="term"
                as="template">
                <li>
                    <span>{{ term.name }}</span>
                    <span v-if="selected" class="list-select">
                        <CheckIcon aria-hidden="true" />
                    </span>
                </li>
            </ListboxOption>
        </ListboxOptions>
    </Listbox>
</template>

<script lang="ts" setup>
import { EnrollmentTerm } from "../logic/Api"
import { ref } from "vue"
import {
    Listbox,
    ListboxButton,
    ListboxOptions,
    ListboxOption,
} from "@headlessui/vue"
import { ChevronUpDownIcon, CheckIcon } from "@heroicons/vue/20/solid"

const terms: Array<EnrollmentTerm> = [
    {
        name: "2223vj",
        start_at: "2023-01-15T23:00:00Z",
        end_at: "2023-08-26T22:00:00Z",
    },
    {
        name: "2223nj",
        start_at: "2022-07-31T22:00:00Z",
        end_at: "2023-03-26T22:00:00Z",
    },
    {
        name: "2122vj",
        start_at: "2022-01-15T23:00:00Z",
        end_at: "2022-08-26T22:00:00Z",
    },
    {
        name: "2122nj",
        start_at: "2021-08-01T22:00:00Z",
        end_at: "2022-03-26T22:00:00Z",
    },
]
const selectedTerm = ref(terms[0])
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
