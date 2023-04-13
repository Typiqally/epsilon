<template>
  <apexcharts
    type="bar"
    height="350"
    :options="chartOptions"
    :series="series"
    v-if="!!hboIDomain"
  />
</template>

<script lang="ts">
import apexcharts from "vue3-apexcharts";

export default {
    name: "KpiMatrix",
    components: {apexcharts},
    props: {
        hboIDomain: {
            default: {}
        }
    },
    watch: {
        hboIDomain() {
            for (const ac in this.hboIDomain.activities) {
                this.chartOptions.xaxis.categories.push(this.hboIDomain.activities[ac].name)
            }


            for (const al in this.hboIDomain.activities) {
                this.series.push({
                    name: this.hboIDomain.architectureLayers[al].name,
                    color: this.hboIDomain.architectureLayers[al].color,
                    data: [
                        Math.random(),
                        Math.random(),
                        Math.random(),
                        Math.random(),
                        Math.random(),
                    ]
                })
            }
        }
    },
    data() {
        return {
            series: [],
            chartOptions: {
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
            },
        }
    }
}
</script>

<style scoped>

</style>
