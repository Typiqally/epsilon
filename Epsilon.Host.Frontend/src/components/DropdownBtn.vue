<template>
    <div class="termselect">
        <Combobox
            v-if="modelValue"
            :model-value="props.modelValue"
            @update:model-value="$emit('update:modelValue', $event)">
            <div class="dropdown">
                <ComboboxInput
                    class="dropdown-input"
                    :display-value="(modelValue) => modelValue.name"
                    @update:model-value="$emit('update:modelValue', $event)" />
                <ComboboxButton class="list-arrow">
                    <ChevronUpDownIcon aria-hidden="true" />
                </ComboboxButton>
            </div>
            <ComboboxOptions class="dropdown-options">
                <div v-if="filteredTerm.length === 0 && query !== ''">
                    Nothing found.
                </div>

                <ComboboxOption
                    v-for="item in items"
                    v-slot="{ active, selected }"
                    :key="item.name"
                    class="dropdown-option"
                    :value="item"
                    as="template">
                    <li>
                        <span>
                            {{ item.name }}
                        </span>
                        <span v-if="selected">
                            <CheckIcon aria-hidden="true" />
                        </span>
                    </li>
                </ComboboxOption>
            </ComboboxOptions>
        </Combobox>
    </div>
</template>

<script lang="ts" setup>
import { defineProps } from "vue"
import { ref, computed } from "vue"
import {
    Combobox,
    ComboboxInput,
    ComboboxButton,
    ComboboxOptions,
    ComboboxOption,
} from "@headlessui/vue"
import { ChevronUpDownIcon, CheckIcon } from "@heroicons/vue/20/solid"

const props = defineProps<{
    items: Array<{ name: string }>
    modelValue: { name: string }
}>()

defineEmits(["update:modelValue"])

const query = ref("")

const filteredTerm = computed(() =>
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

<style scoped>
.termselect {
    background-color: #fff;
    height: 3rem;
    width: 10rem;
    border-radius: 5px;
}

.list-arrow {
    background-color: transparent;
    border: none;
    border-radius: 0;
}
.list-arrow:hover,
.list-arrow:focus,
.list-arrow:active {
    border: none;
    outline: none;
}
.list-arrow svg,
.list-select svg {
    max-height: 30px;
}

.dropdown {
    display: grid;
    grid-template-columns: 1fr 60px;
    align-items: center;
    border: none;
    font-weight: 400;
    text-align: left;
    min-width: 6rem;
    max-height: 3rem;
}

.dropdown-input {
    border: none;
    outline: none;
    padding: 0.75rem;
    height: 3rem;
    width: 6rem;
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
