<template>
    <apexcharts type="bar" :options="chartOptions" :series="series"></apexcharts>
</template>

<script lang="ts" setup>
import {HboIDomain, ProfessionalTaskOutcome} from "@/logic/Api";
import {watch} from "vue";

const props = defineProps<{
    domain: HboIDomain
    data: ProfessionalTaskOutcome[]
}>()

const series = [{
    name: '',
    data: [44, 55, 41, 67]
}
]
const chartOptions = {
    annotations: {
        yaxis: [
            {
                y: 40,
                borderColor: 'red',
                strokeDashArray: 0,
                label: {
                    borderColor: 'red',
                    style: {
                        color: '#fff',
                        background: 'red'
                    },
                    text: 'Mastery'
                }
            }
        ]
    },
    colors: ["#A8D08D"],
    chart: {
        type: 'bar',
        stacked: true,
        toolbar: {
            show: false
        },
        zoom: {
            enabled: false
        }
    },
    dataLabels: {
        enabled: false
    },
    plotOptions: {
        bar: {
            horizontal: false,
            borderRadius: 4,
            dataLabels: {}
        },
    },
    xaxis: {
        type: 'string',
        categories: [],
    },
    yaxis: {
        show: false,
    },
    legend: {
        position: 'bottom'
    },
    fill: {
        opacity: 1
    },
    tooltip: {
        enabled: false
    }
}

watch
(() => props.domain, () => {
    for (const pd in props.domain.professionalSkills) {
        chartOptions.xaxis.categories.push(props.domain.professionalSkills?.[pd].name)
    }
})
</script>

<style scoped>

</style>