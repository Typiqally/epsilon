<template>
  <apexcharts
    type="bar"
    height="320"
    width="300"
    :options="chartOptions"
    :series="series"
  />
</template>

<script lang="ts" setup>
import apexcharts from "vue3-apexcharts";
import {HboIDomain, ProfessionalTaskOutcome} from "@/logic/Api";

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

props.domain.professionalSkills.forEach(skill => {
    chartOptions.xaxis.categories.push(skill.shortName)
})
</script>

<style scoped>

</style>
