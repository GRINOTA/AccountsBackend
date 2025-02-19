<template>
    <div ref="chart" :style="{width: '80%', height: '800px'}"></div>
</template>
<script>
    import * as echarts from 'echarts'

    export default {
        name: 'LineChart',
        props: ['options'],
        data() {
            return {
                chart: null
            }
        },
        mounted() {
            if(this.options) {
                this.initChart(this.options)
            }
        },
        beforeUnmount() {
            if(this.chart) {
                this.chart.dispose()
            }
        },
        watch: {
            options: {
                handler(newOptions) {
                    if(this.chart) {
                        this.chart.setOption(newOptions, true)
                    } else {
                        this.initChart(newOptions)
                    }
                },
                deep: true
            }
        },
        methods: {
            initChart(options) {
                if(this.chart) {
                    this.chart.dispose()
                }
                this.chart = echarts.init(this.$refs.chart)
                this.chart.setOption(options, true)
                this.$emit('chart-ready')
                console.log("LineChart mounted"); 
            }    
        }
    }
</script>