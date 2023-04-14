<template>
  <apexchart
    type="bar"
    height="350"
    :options="chartOptions"
    :series="series"
  />
</template>

<script lang="ts" setup>
import apexchart from "vue3-apexcharts";
import {HboIDomain} from "@/logic/Api";
import {ref, watch} from "vue";
const series = []
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
    colors:[],
        chart: {
        type: 'bar',
            height: 350,
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
                dataLabels: {
            }
        },
    },
    xaxis: {
        type: 'string',
            categories: [],
    },
    yaxis:{
        show: false,
    },
    legend: {
        position: 'bottom'
    },
    fill: {
        opacity: 1
    },
    tooltip:{
        enabled: false
    }
}

const props = defineProps<{
    domain: HboIDomain
}>()

for (const ac in props.domain.activities) {
    chartOptions.xaxis.categories.push(props.domain.activities[ac].name as never)
}

for (const i in props.domain.architectureLayers) {
    const ar = props.domain.architectureLayers[i]
    series.push({
        name: ar.name,
        color: ar.color,
        data: [
            Math.random(),
            Math.random(),
            Math.random(),
            Math.random(),
            Math.random(),
        ]
    })
}
</script>

<style scoped>

</style>
