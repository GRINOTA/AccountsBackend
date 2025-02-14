<template>
    <div>
        <div>
            <legend>Отчет по движениям средств</legend>
            <div class="mb-3">
                <label for="select" class="form-label">Конвертировать в валюту</label>
                <select id="select" class="form-select" v-model.number="selectedCurrency">
                    <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                        {{currency.codeCurrency}}
                    </option>
                </select>
            </div>          

            <div class="row justify-content-center">
                <line-chart :options="lineChartOptions"></line-chart>
                <!-- <line-chart :options="barChartOption"></line-chart> -->
            </div>
        </div>
        <div>
            
        </div>
    </div>
</template>

<script>
    import LineChart from '../components/LineChart.vue'
    import TransactionService from '../services/transactionService';
    import moment from 'moment'
    import CurrencyService from '../services/currencyService'
    import CurrencyRatesService from '../services/currencyRatesService'

    export default {
        components: { 
            LineChart
        },
        data() {
            return {
                selectedCurrency:  1,
                currencies: [],

                lineChartOptions: {
                    xAxis: {
                        type: 'time',
                        name: 'Даты'
                        // axisLabel: {
                        //     formatter: 'yyyy-MM-dd'
                        // }
                    },
                    yAxis: {
                        type: 'value',
                        name: 'Баланс'
                        // axisLabel: {
                        //     formatter: 'value'
                        // }
                    },
                    dataZoom: [
                        {
                            type: 'inside',
                            start: 0,
                            end: 100,
                            xAxisIndex: 'none',
                            yAxisIndex: 0
                        },
                        // {
                        //    type: 'slider',
                        //    start: 0,
                        //    end: 100,
                        //    xAxisIndex: 'none',
                        //    yAxisIndex: 0
                        // },
                        {
                            type: 'inside',
                            start: 0,
                            end:  100
                        },
                        // {
                        //    type: 'slider',
                        //    start: 0,
                        //    end: 100
                        // }
                    ],
                    tooltip: {
                        trigger: 'axis',
                        formatter: (params) => {
                            let tooltipContent = `Дата: ${params[0].data[0]}<br>`
                            params.forEach(item => {
                                tooltipContent += `Счёт: ${item.seriesName}<br>`
                                tooltipContent += `Баланс: ${item.data[1].toFixed(2)}<br>`
                            });
                            return tooltipContent
                        }
                    },
                    legend: {
                        data: []
                    },
                    series: [],
                    currencyRates: null
                }
            }
        },
        watch: {
            selectedCurrency: {
                handler: async function() {
                    await this.getTransaction()
                }
            }
        },
        async mounted() {
            await this.getCurrencies()
            await this.getTransaction()
        },
        methods: {
            async getCurrencies() {
                this.currencies = await CurrencyService.getAllCurrency()
            },
            async getTransaction() {

                const transactions = await TransactionService.getTransaction()
                
                const rates = await CurrencyRatesService.getRateByTargetRate(this.selectedCurrency)

                this.currencyRates = rates && rates.rate ? rates.rate : null
                
                const lineChartOptions = {...this.lineChartOptions}
                lineChartOptions.series = []

                for (const account of transactions) {
                    const seriesData = []

                    let currentBalance = account.balance
                    console.log(`CurrentBalance 1: ${currentBalance}`)
                    for (const movement of account.movements) {
                        let amount = movement.amount
                        console.log(`Amount 1: ${amount}`)
                        
                        

                        console.log(`CurrentBalance 2: ${currentBalance}`)

                        if(account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                            amount = amount * this.currencyRates
                            
                            // currentBalance = currentBalance * this.currencyRates
                            console.log(`Курс: ${this.currencyRates}`)
                            console.log(`CurrentBalance 3: ${currentBalance}`)
                            
                        }
                        currentBalance -= amount  
                        seriesData.push([
                            moment(movement.date).format('YYYY-MM-DD'),
                            currentBalance,
                            movement.recipientAccountNumber
                        ])
                    }

                    if(seriesData.length > 0) {
                        lineChartOptions.series.push({
                            name: `${account.accountNumber} (${account.currency})`,  
                            data: seriesData,
                            type: 'line'  
                        })
                    }
                }

                lineChartOptions.legend.data = lineChartOptions.series.map(series => series.name)
                this.lineChartOptions = lineChartOptions
                
                // this.lineChartOptions.series = transactions.map(account => {
                //     const seriesData = account.movements.map(movement => [
                //         moment(movement.date).format('YYYY-MM-DD'),
                //         movement.amount,
                //         movement.recipientAccountNumber
                //     ])

                //     return {
                //         name: `${account.accountNumber} (${account.currency})`,
                //         data: seriesData,
                //         type: 'line'
                //     }
                // })

                // this.lineChartOptions.legend.data = this.lineChartOptions.series.map(series => series.name)
            }
        }
    }
</script>