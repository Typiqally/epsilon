<template>
    <apexcharts type="bar" :options="chartOptions" :series="series" v-if="!!hboIDomain"></apexcharts>
</template>

<script lang="ts">
import apexcharts from "vue3-apexcharts";

export default {
    name: "PersonalDevelopmentMatrix",
    components: {apexcharts},
    props: {
        hboIDomain: {
            default: {}
        }
    },
    watch: {
        hboIDomain() {
            for (const pd in this.hboIDomain.professionalSkills) {
                console.log(pd, this.hboIDomain.professionalSkills[pd].name)
                this.chartOptions.xaxis.categories.push(this.hboIDomain.professionalSkills[pd].name)
            }
        }
    },
    data() {
        return {
            series: [{
                name: '',
                data: [44, 55, 41, 67]
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
            },
        }
    }
}
</script>

<style scoped>

</style>