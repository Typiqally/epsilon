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
                v-slot="{ selected }"
                :key="item.name"
                class="dropdown-option"
                :value="item"
                as="template">
                <li>
                    <span>{{ item.name }}</span>
                    <span v-if="selected">
                        <CheckIcon aria-hidden="true" />
                    </span>
                </li>
            </ListboxOption>
        </ListboxOptions>
    </Listbox>
</template>

<script lang="ts" setup>
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
.termselect {
    min-width: 8rem;
    grid-template-columns: 1fr 20px;
    gap: 1rem;
    border-radius: 7px;
}

.list-arrow {
    background-color: transparent;
    border: none;
    border-radius: 0;
    align-self: flex-end;
}
.list-arrow:hover,
.list-arrow:focus,
.list-arrow:active,
.termselect {
    border: none;
    outline: none;
}
.list-arrow svg {
    height: 20px;
    max-height: 30px;
}

.dropdown {
    display: grid;
    align-items: center;
    border: none;
    font-weight: 400;
    text-align: left;
    min-width: 6rem;
    height: 3rem;
    max-height: 3rem;
}

.dropdown-options {
    list-style-type: none;
    position: absolute;
    background-color: #fff;
    padding: 1rem;
    min-width: 10rem;
    border-radius: 6px;
    border: 1px solid #d8d8d8;
    text-align: left;
    z-index: 999;
}

.dropdown-option {
    padding: 15px;
    cursor: pointer;
    display: grid;
    grid-template-columns: 1fr 25px;
    text-align: left;
}
</style>
