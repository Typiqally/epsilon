<template>
  <ApexChart
    type="bar"
    height="320"
    width="200"
    :options="chartOptions"
    :series="series"
  />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts";
import {DecayingAveragePerSkill, IHboIDomain} from "../logic/Api";
import {onMounted} from "vue";

const props = defineProps<{
    domain: IHboIDomain
    data: DecayingAveragePerSkill[]
}>()

const series: Array<{ name: string, data: Array<number | string> }> = []
const chartOptions = {
    annotations: {
        yaxis: [
            {
                y: 3,
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
        enabled: true
    }
}

onMounted(() => {
    // Setup categories
    const professionalSkills = props.domain.professionalSkills

    if (professionalSkills != null) {
        professionalSkills.forEach(s => {
            chartOptions.xaxis.categories.push(s.shortName as never)
        })
    }

    // Add data
    series.push({
        name: "Decaying Average",
        data: props.data.map(decayingAverage => decayingAverage.decayingAverage?.toFixed(2) as string),
    })
})
</script>
