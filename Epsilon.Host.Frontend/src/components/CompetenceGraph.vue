<template>
    <ApexChart
        :options="chartOptions"
        :series="series"
        height="350"
        type="bar"
        width="750" />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import { IHboIDomain, ProfessionalTaskResult } from "../logic/Api"
import { onMounted } from "vue"
import { DecayingAveragePerActivity, DecayingAveragePerLayer } from "/@/logic/DecayingAverageLogic"

const series: Array<{
    name: string
    color: string
    data: Array<string | number> | undefined
}> = []
const chartOptions = {
    annotations: {
        yaxis: [
            {
                y: 3,
                borderColor: "red",
                strokeDashArray: 0,
                label: {
                    borderColor: "red",
                    style: {
                        color: "#fff",
                        background: "red",
                    },
                    text: "Mastery",
                },
            },
        ],
    },
    colors: [],
    chart: {
        type: "bar",
        height: 350,
        stacked: true,
        toolbar: {
            show: true,
        },
        zoom: {
            enabled: false,
        },
    },
    dataLabels: {
        enabled: false,
    },
    plotOptions: {
        bar: {
            horizontal: false,
            borderRadius: 4,
            dataLabels: {},
        },
    },
    xaxis: {
        type: "string",
        categories: [],
    },
    yaxis: {
        show: false,
    },
    legend: {
        position: "bottom",
    },
    fill: {
        opacity: 1,
    },
    tooltip: {
        enabled: true,
    },
}

const props = defineProps<{
    domain: IHboIDomain
    data: ProfessionalTaskResult[]
}>()

onMounted(() => {
    // Setup categories
    const activities = props.domain.activities
    const DecayingAverage: Record<string, DecayingAveragePerLayer> = {}

    if (activities != null) {
        activities.forEach((s) => {
            chartOptions.xaxis.categories.push(s.name as never)
        })
    }

    props.domain.architectureLayers?.forEach((l) => {
        if (l.id !== undefined) {
            DecayingAverage[l?.id] = {
                architectureLayer: l.id,
                layerActivities: [] as DecayingAveragePerActivity[],
            }
        }
    })

    const resultsByOutcome = groupBy(
        props.data,
        (r: ProfessionalTaskResult) => r.outcomeId,
    )

    for (const outcomeId in resultsByOutcome) {
        const resultByOutcome = resultsByOutcome[outcomeId]
        let totalGradeScore = 0.0

        const recentResult = resultByOutcome.at(0)
        const pastResults = resultByOutcome.slice(1, resultByOutcome.length - 1)
        if (pastResults.length > 0) {
            pastResults.forEach((r) => (totalGradeScore += r.grade))
            totalGradeScore =
                (totalGradeScore / pastResults.length) * 0.35 +
                recentResult.grade * 0.65
        } else {
            totalGradeScore = recentResult.grade
        }

        DecayingAverage[recentResult.architectureLayer].layerActivities.push({
            activity: recentResult.activity,
            decayingAverage: totalGradeScore,
        })
    }

    for (const decayingAverageKey in DecayingAverage) {
        series.push({
            name: props.domain.architectureLayers?.at(decayingAverageKey).name,
            color: props.domain.architectureLayers?.at(decayingAverageKey).color,
            data: countBasedOnActivities(DecayingAverage[decayingAverageKey].layerActivities),
        })
    }
})

function countBasedOnActivities(layerActivities: DecayingAveragePerActivity[]) {
    const list = []
    for (let i = 0; i <= 4; i++) {
        let score = 0
        const selectedActivities = layerActivities.filter((ac) => ac.activity === i)
        selectedActivities.map((ac) => (score += ac.decayingAverage))
        list.push(score)
    }
    console.log(list)
    return list
}

function groupBy<T>(arr: T[], fn: (item: T) => any): Record<string, T[]> {
    return arr.reduce<Record<string, T[]>>((prev, curr) => {
        const groupKey = fn(curr)
        const group = prev[groupKey] || []
        group.push(curr)
        return { ...prev, [groupKey]: group }
    }, {})
}
</script>
