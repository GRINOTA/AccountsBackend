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
                        date: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            data: [120, 200, 150, 80, 70, 110],
                            type: 'line'
                        }
                    ]
                }
            }
        }
    }
</script>