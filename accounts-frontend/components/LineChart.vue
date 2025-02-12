<template>
    <div ref="chart" :style="{width: '600px', height: '400px'}"></div>
</template>
<script>
    import * as echarts from 'echarts'

    export default {
        name: 'LineChart',
        props: {
            options: {
                type: Object,
                required: true
            }
        },
        mounted() {
            const chart = echarts.init(this.$refs.chart)
            chart.setOption(this.options)
        },
        watch: {
            options: {
                handler(newOptions) {
                    const chart = echarts.init(this.$refs.chart)
                    chart.setOption(newOptions)
                },
                deep: true
            }
        }
    }
</script>