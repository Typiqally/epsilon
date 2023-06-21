<template>
    <ApexChart
        :options="chartOptions"
        :series="series"
        height="350"
        type="bar"
        class="competence-graph"
        width="490" />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import { IHboIDomain, ProfessionalTaskResult } from "../logic/Api"
import { onMounted, watchEffect } from "vue"
import {
    DecayingAverageLogic,
    DecayingAveragePerLayer,
} from "../logic/DecayingAverageLogic"

let series: Array<{
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
        show: false,
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

function loadChartData(): void {
    series = []
    chartOptions.xaxis.categories = []
    if (props.domain.activities != null) {
        props.domain.activities.forEach((s) => {
            chartOptions.xaxis.categories.push(s.shortName as never)
        })
    }

    DecayingAverageLogic.getAverageTaskOutcomeScores(
        props.data,
        props.domain
    ).map((layer: DecayingAveragePerLayer) => {
        const ar = props.domain.architectureLayers?.at(layer.architectureLayer)
        series.push({
            name: ar?.name,
            color: ar?.color,
            data: layer.layerActivities.map((ac) =>
                ac.decayingAverage.toFixed(3)
            ),
        })
    })
}

onMounted(() => {
    loadChartData()
})
watchEffect(() => loadChartData())
</script>

<style scoped>
.competence-graph {
    margin-left: auto;
}
</style>
