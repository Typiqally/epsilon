<template>
    <apexcharts type="bar" height="350" :options="chartOptions" :series="series"></apexcharts>
</template>

<script lang="ts">
import apexcharts from "vue3-apexcharts";

export default {
    name: "KpiMatrix",
    components: {apexcharts},
    data() {
        return {
            series: [{
                name: 'User interaction',
                data: [44, 55, 41, 67, 22]
            }, {
                name: 'Infrastructure',
                data: [13, 23, 20, 8, 13]
            }, {
                name: 'Organisational processes',
                data: [11, 17, 15, 15, 21]
            }, {
                name: 'Software',
                data: [21, 7, 25, 13, 22]
            }, {
                name: 'Hardware interfacing',
                data: [21, 7, 25, 13, 22]
            }
            ],
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
                colors:["#E29C53","#6EA7D4","#D16557", "#96B9C0","#8D9292" ],
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
                    categories: [
                        'Analyse',
                        'Advise',
                        'Design',
                        'Realise',
                        'Manage & Control'
                    ],
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