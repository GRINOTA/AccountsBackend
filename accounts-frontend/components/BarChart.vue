<template>
    <div ref="chart" :style="{width: '100%', height: '1000px'}"></div>
</template>

<script>
    import * as echarts from 'echarts'

    export default {
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
