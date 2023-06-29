<template>
    <div class="combobox-wrapper">
        <Combobox
            :model-value="props.modelValue"
            @update:model-value="$emit('update:modelValue', $event)">
            <div class="dropdown">
                <ComboboxInput
                    class="dropdown-input"
                    :display-value="(person) => person.name"
                    @change="query = $event.target.value" />
                <ComboboxButton class="list-arrow">
                    <ChevronUpDownIcon aria-hidden="true" />
                </ComboboxButton>
            </div>
            <ComboboxOptions class="dropdown-options">
                <div
                    v-if="filteredItems.length === 0 && query !== ''"
                    class="dropdown-option">
                    No students found
                </div>

                <ComboboxOption
                    v-for="(item, id) in filteredItems"
                    :key="id"
                    v-slot="{ selected, active }"
                    as="template"
                    :value="item"
                    class="dropdown-option">
                    <li
                        :class="{
                            'dropdown-option-active': active,
                            '': !active,
                        }">
                        <span>
                            {{ item.name }}
                        </span>
                        <span v-if="selected" class="dropdown-select-icon">
                            <CheckIcon aria-hidden="true" />
                        </span>
                    </li>
                </ComboboxOption>
            </ComboboxOptions>
        </Combobox>
    </div>
</template>

<script lang="ts" setup>
import { computed, defineProps, ref } from "vue"
import {
    Combobox,
    ComboboxButton,
    ComboboxInput,
    ComboboxOptions,
    ComboboxOption,
} from "@headlessui/vue"
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/vue/20/solid"

const props = defineProps<{
    items: Array<{ name: string }>
    modelValue: { name: string }
}>()

const query = ref("")
defineEmits(["update:modelValue"])

const filteredItems = computed(() =>
    query.value === ""
        ? props.items
        : props.items.filter((item) =>
              item.name
                  .toLowerCase()
                  .replace(/\s+/g, "")
                  .includes(query.value.toLowerCase().replace(/\s+/g, ""))
          )
)
</script>

<style scoped lang="scss">
.combobox-wrapper {
    background-color: #fff;
    height: 3rem;
    border-radius: 7px;
    margin-right: 2rem;
    min-width: 15rem;
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

        &-active {
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
