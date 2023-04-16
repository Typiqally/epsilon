<template>
  <div class="enrollment-term-buttons">
    <button
      v-for="term of props.terms"
      :key="term"
      class="btn"
      @click="emitTermSelectedEvent(term)"
    >
      {{ term.name }}
    </button>
  </div>
</template>

<script lang="ts" setup>
import {EnrollmentTerm} from "../logic/Api";

const props = defineProps<{
    terms: EnrollmentTerm[]
}>()

const emit = defineEmits(['onTermSelected'])

function emitTermSelectedEvent(term: EnrollmentTerm): void {
    emit('onTermSelected', term)
}

</script>

<style scoped>
.btn {
    background: #663366;
    color: #ffffff;
    border: 1px solid #663366;
    border-radius: 3px;
    text-align: center;
    margin: 1rem;
}

.btn:hover {
    background: #723972;
}

.enrollment-term-buttons {
    display: flex;
    flex-direction: column;
    justify-content: center;
    width: fit-content;
}

</style>
