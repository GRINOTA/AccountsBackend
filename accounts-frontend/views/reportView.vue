<template>
    <div>
        <line-chart :options="chartOptions"></line-chart>
    </div>
</template>

<script>
import LineChart from '../components/LineChart.vue'
// import TransactionService from '../services/transactionService';

    export default {
        components: { 
            LineChart 
        },
        data() {
            return {
                chartOptions: {
                    xAxis: {
                        type: 'category',
                        date: ['Mon']
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            data: [120, 130, 453],
                            type: 'line'
                        },
                        {
                            data: [123, 13, 453],
                            type: 'line'
                        }
                    ]
                }
            }
        }
    }
</script>