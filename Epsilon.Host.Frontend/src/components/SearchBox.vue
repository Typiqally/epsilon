<template>
    <div class="combobox-wrapper">
        <Combobox
            :model-value="props.modelValue"
            @update:model-value="$emit('update:modelValue', $event)">
            <div class="profileselect dropdown">
                <ComboboxInput
                    class="dropdown-input"
                    :display-value="(person) => person.name"
                    @change="query = $event.target.value" />
                <ComboboxButton class="list-arrow">
                    <ChevronUpDownIcon aria-hidden="true" />
                </ComboboxButton>
            </div>
            <ComboboxOptions class="dropdown-options">
                <div v-if="filteredItems.length === 0 && query !== ''">
                    Nothing found.
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
                            'active-list-item': active,
                            '': !active,
                        }">
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

<style scoped>
.profileselect {
    grid-template-columns: 1fr 60px;
}

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
}

.list-arrow:hover,
.list-arrow:focus,
.list-arrow:active {
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

.dropdown-input {
    border: none;
    outline: none;
    padding: 0.75rem;
    height: 3rem;
    min-width: 6rem;
    font-size: 1rem;
    border-radius: 6px;
}

.dropdown-options {
    list-style-type: none;
    position: absolute;
    background-color: #fff;
    padding: 0.5rem;
    min-width: 10rem;
    border-radius: 6px;
    border: 1px solid #d8d8d8;
    text-align: left;
    z-index: 999;
}

.active-list-item {
    background-color: #f2f3f8;
}

.dropdown-option {
    padding: 1rem 1.5rem;
    cursor: pointer;
    display: grid;
    grid-template-columns: 1fr 25px;
    text-align: left;
}
</style>
