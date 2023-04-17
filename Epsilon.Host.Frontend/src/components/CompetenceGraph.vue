<template>
  <ApexChart
    type="bar"
    height="350"
    width="750"
    :options="chartOptions"
    :series="series"
  />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts";
import {DecayingAveragePerLayer, IHboIDomain} from "../logic/Api";
import {onMounted} from "vue";

const series: Array<{ name: string, color: string, data: Array<string | number> | undefined }> = []
const chartOptions = {
    annotations: {
        yaxis: [
            {
                y: 9,
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
    colors: [],
    chart: {
        type: 'bar',
        height: 350,
        stacked: true,
        toolbar: {
            show: true
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

const props = defineProps<{
    domain: IHboIDomain,
    data: DecayingAveragePerLayer[]
}>()

onMounted(() => {
    // Setup categories
    const activities = props.domain.activities

    if (activities != null) {
        activities.forEach(s => {
            chartOptions.xaxis.categories.push(s.name as never)
        })
    }

    // Add data
    const layers = props.domain.architectureLayers

    if (layers != null) {
        props.data.forEach(layerDecayingAverage => {
            const layer = layers.find(layer => layer.id == layerDecayingAverage.architectureLayer)

            if (layer != undefined) {
                series.push({
                    name: layer.name as string,
                    color: layer.color as string,
                    data: layerDecayingAverage.layerActivities?.map(decayingAverage => decayingAverage.decayingAverage?.toFixed(2) as string)
                })
            }
        })
    }
})

</script>
