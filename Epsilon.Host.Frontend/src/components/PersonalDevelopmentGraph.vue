<template>
    <ApexChart
        :options="chartOptions"
        :series="series"
        height="350"
        type="bar"
        width="200" />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import { IHboIDomain, ProfessionalSkillResult } from "../logic/Api"
import { onMounted, watch } from "vue"
import { DecayingAverageLogic } from "../logic/DecayingAverageLogic"

const props = defineProps<{
    domain: IHboIDomain
    data: ProfessionalSkillResult[]
}>()

let series: Array<{ name: string; data: Array<number | string> }> = []
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
    colors: ["#A8D08D"],
    chart: {
        type: "bar",
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

onMounted(() => {
    loadChartData()
})

function loadChartData(): void {
    series = []
    chartOptions.xaxis.categories = []
    if (props.domain.professionalSkills != null) {
        props.domain.professionalSkills.forEach((s) => {
            chartOptions.xaxis.categories.push(s.name as never)
        })
    }

    console.log()

    // Add data
    series.push({
        name: "Score",
        data: DecayingAverageLogic.GetAverageSkillOutcomeScores(
            props.data,
            props.domain
        )?.map((d) => d.decayingAverage.toFixed(3)),
    })
}

watch(() => loadChartData())
</script>
