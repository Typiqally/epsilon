<template>
    <td
        class="competence-profile-data"
        :style="{
            backgroundColor: cellColor(props.kpis)?.color,
        }"
        :class="{
            'cell-changed': hasChanged,
        }">
        <div class="cell-value">{{ props.kpis.length }}</div>
    </td>
</template>

<script lang="ts" setup>
import { watch, ref } from "vue"
import { MasteryLevel, ProfessionalTaskResult } from "../logic/Api"

const hasChanged = ref(false)

const props = defineProps<{
    name: string
    kpis: ProfessionalTaskResult[]
    levels: MasteryLevel[]
}>()

watch(
    () => props.kpis,
    (newKpis, oldKpis) => {
        if (props.kpis.length === 0) {
            return
        }
        hasChanged.value = newKpis.length != oldKpis.length
        if (hasChanged.value) {
            setTimeout(() => {
                hasChanged.value = false
            }, 700)
        }
    }
)

function cellColor(kpis): MasteryLevel | undefined {
    const sortedKpis = kpis.sort(
        (a, b) =>
            props.levels.find(
                (masteryLevel) => masteryLevel.id == b?.masteryLevel
            ).level - props.levels.find((ml) => ml.id == a?.masteryLevel).level
    )

    return props.levels.find(
        (masteryLevel) => masteryLevel.id == sortedKpis[0]?.masteryLevel
    )
}
</script>

<style scoped>
.cell-changed .cell-value {
    animation: 700ms 1 ease cell-value-changed;
}

@keyframes cell-value-changed {
    0% {
        transform: scale(130%);
        transform: translate3d(0, 1.75em, -10em) rotateY(-225deg);
        opacity: 0;
    }
    20% {
        opacity: 0;
    }
    100% {
        transform: scale(100%);
        transform: translate3d(0, 0, 0) rotateY(0deg);
    }
}
</style>
