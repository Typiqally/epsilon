<template>
    <div class="list-box-wrapper">
        <Listbox
            v-if="modelValue"
            :model-value="props.modelValue"
            @update:model-value="$emit('update:modelValue', $event)">
            <ListboxButton class="dropdown">
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
                        <span v-if="selected" class="dropdown-select-icon">
                            <CheckIcon aria-hidden="true" />
                        </span>
                    </li>
                </ListboxOption>
            </ListboxOptions>
        </Listbox>
    </div>
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

<style scoped lang="scss">
.list-box-wrapper {
    background-color: #fff;
    height: 3rem;
    border-radius: 7px;
    margin-right: 2rem;
    min-width: 6rem;
}

.list-arrow {
    background-color: transparent;
    border: none;
    border-radius: 0;
    align-self: flex-end;
    margin-left: 20px;

    &:hover,
    &:focus,
    &:active {
        border: none;
        outline: none;
    }

    svg {
        height: 20px;
        max-height: 30px;
    }
}

.dropdown {
    position: relative;
    display: flex;
    align-items: center;
    justify-content: space-between;
    border: none;
    font-weight: 400;
    text-align: left;
    width: 100%;
    height: 3rem;
    max-height: 3rem;
    background: white;
    outline: none;

    &-input {
        border: none;
        outline: none;
        padding: 0.75rem;
        height: 3rem;
        min-width: fit-content;
        font-size: 1rem;
        border-radius: 6px;
    }

    &-options {
        list-style-type: none;
        position: relative;
        background-color: #fff;
        width: 100%;
        border-radius: 6px;
        border: 1px solid #d8d8d8;
        text-align: left;
        z-index: 999;
    }

    &-option {
        padding: 1rem 1.5rem;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: space-between;

        &:hover {
            background-color: #f2f3f8;
        }
    }

    &-select-icon {
        padding-left: 4px;
        width: 20px;
        height: 20px;
    }
}
</style>
