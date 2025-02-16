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
            <div>
                <VueDatePicker v-model="date" range></VueDatePicker>
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
    import VueDatePicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css'

    export default {
        components: { 
            LineChart,
            VueDatePicker
        },
        data() {
            return {
                date: [],
                selectedCurrency:  1,
                currencies: [],

                lineChartOptions: {
                    xAxis: {
                        type: 'time',
                        name: 'Даты',
                        // boundaryGap: false
                        // axisLabel: {
                        //     formatter: `yyyy-MM-dd`
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
                        {
                            type: 'inside',
                            start: 0,
                            end:  100
                        },
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
                    currencyRates: null,
                    transactions: []
                }
            }
        },
        watch: {
            selectedCurrency: {
                handler: async function() {
                    await this.getTransaction()
                    await this.updateChart();
                }
            },
            date: {
                handler: async function() {
                    // await this.getTransaction()
                    await this.updateChart();
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

                this.transactions = await TransactionService.getTransaction()
                
                const rates = await CurrencyRatesService.getRateByTargetRate(this.selectedCurrency)

                this.currencyRates = rates && rates.rate ? rates.rate : null
                
                
                this.updateChart()
            },
            updateChart() {
                const lineChartOptions = { ...this.lineChartOptions };
                let startDate, endDate

                lineChartOptions.series = [];

                if (!this.date || this.date.length !== 2) { 
                    lineChartOptions.legend.data = [];
                    delete lineChartOptions.xAxis.min; // Remove min property
                    delete lineChartOptions.xAxis.max; // Remove max property
                    this.lineChartOptions = lineChartOptions; 
                } else {
                    startDate = moment(this.date[0]);
                    endDate = moment(this.date[1]);

                    lineChartOptions.xAxis.min = startDate.valueOf();
                    lineChartOptions.xAxis.max = endDate.valueOf();
                }

                

                // lineChartOptions.series = []

                for (const account of this.transactions) {
                    const seriesData = []

                    let currentBalance = account.balance

                    if (account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                        currentBalance = currentBalance * this.currencyRates
                    }

                    // фильтр движений по дате
                    const filteredMovements = (this.date && this.date.length === 2) ? account.movements.filter(movement => {
                        const movementDate = moment(movement.date);
                        return movementDate.isBetween(startDate, endDate, null, '[]');
                    }) : account.movements;

                    const sortedMovements = filteredMovements.sort((a, b) => {
                        const dateA = moment(a.date)
                        const dateB = moment(b.date)
                        return dateB - dateA
                    });

                    for (const movement of sortedMovements) {
                        let amount = movement.amount

                        if(account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                            amount = amount * this.currencyRates
                        }

                        currentBalance -= amount;

                        seriesData.push([
                            moment(movement.date).valueOf(),
                            currentBalance,
                            movement.recipientAccountNumber,
                            amount
                        ])
                    }

                    console.log(seriesData);
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
            }
        }
    }
</script>