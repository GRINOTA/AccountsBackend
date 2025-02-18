<template>
    <div>
        <div>
            <!-- <p class="h3">Отчет по движениям средств</p> -->
            <legend>Отчет по движениям средств</legend>
            <div class="mb-3">
                <label for="selectCurrency" class="form-label">Конвертировать отчет в валюту</label>
                <select id="selectCurrency" class="form-select" v-model.number="selectedCurrency">
                    <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                        {{currency.codeCurrency}}
                    </option>
                </select>
            </div>
            <div>  
                <p class="h5">Фильтрация</p>
                <div> 
                    <label for="selectDate" class="form-label">По датам</label>
                    <VueDatePicker id="selectDate" v-model="date" range></VueDatePicker>
                </div>
                <div> 
                    <label for="selectCurrencyFiltr" class="form-label">По валютам</label>
                    <select id="selectCurrencyFiltr" class="form-select" v-model.number="selectedCurrencyFilter">
                        <option value="all">ВСЕ</option>
                        <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                            {{currency.codeCurrency}}
                        </option>
                    </select>
                </div> 
            </div>
                  
            <div class="row justify-content-center">
                <line-chart :options="lineChartOptions"></line-chart>
                <!-- <bar-chart :options="barChartOptions"></bar-chart> -->
            </div>
        </div>
    </div>
</template>

<script>
    import LineChart from '../components/LineChart.vue'
    // import BarChart from '../src/components/BarChart.vue'
    import TransactionService from '../api/services/transactionService';
    import moment from 'moment'
    import CurrencyService from '../api/services/currencyService'
    import CurrencyRatesService from '../api/services/currencyRatesService'
    import VueDatePicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css'

    export default {
        components: { 
            LineChart,
            // BarChart,
            VueDatePicker
        },
        data() {
            return {
                transactions: [],

                selectedCurrency:  1,
                selectedCurrencyFilter: null,
                currencies: [],
                currencyRates: null,

                date: [],
                
                lineChartOptions: {
                    xAxis: {
                        type: 'time',
                        name: 'Дата'
                    },
                    yAxis: {
                        type: 'value',
                        name: 'Баланс'
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
                            let tooltipContent = `Дата: ${moment(params[0].data[0]).format("YYYY-MM-DD HH:mm:ss")}<br>`
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
                    series: []
                },
                // barChartOptions: {
                //     xAxis: {
                //         tipe: 'time',
                //         name: "Дата"
                //         // axisLabel: {
                //         //     // formatter: (value) => moment(value).format('YYYY-MM-DD')
                //         // }
                //     },
                //     yAxis: {
                //         type: 'value',
                //         name: 'Сумма'
                //     },
                //     series: []
                // }
            }
        },
        watch: {
            selectedCurrency: {
                handler: async function() {
                    await this.getTransaction()
                    await this.updateLineChart();
                    // await this.updateBarChart()
                }
            },
            date: {
                handler: async function() {
                    await this.updateLineChart();
                    // await this.updateBarChart()
                }
            },
            selectedCurrencyFilter: {
                handler: async function() {
                    await this.updateLineChart()
                }
            }
        },
        async mounted() {
            await this.getCurrencies()
            await this.getTransaction()
        },

        methods: {
            async getCurrencies() {
                try {
                    this.currencies = await CurrencyService.getAllCurrency()
                } catch(error) {
                    this.$toast.error(`${error.response.status} ${error.response.statusText}`)
                }
            },
            async getTransaction() {
                try {
                    this.transactions = await TransactionService.getTransaction()
                    const rates = await CurrencyRatesService.getRateByTargetRate(this.selectedCurrency)
                    this.currencyRates = rates && rates.rate ? rates.rate : null
                } catch(error) {
                    this.$toast.error(`${error.response.status} ${error.response.statusText}`)
                }
                
                this.updateLineChart()
                // this.updateBarChart()
            },
            updateLineChart() {
                const lineChartOptions = { ...this.lineChartOptions };
                let startDate, endDate

                lineChartOptions.series = [];
 
                if (!this.date || this.date.length !== 2) { 
                    lineChartOptions.legend.data = [];
                    delete lineChartOptions.xAxis.min;
                    delete lineChartOptions.xAxis.max;
                    this.lineChartOptions = lineChartOptions; 

                    startDate = moment().subtract(7,'days').startOf('day')
                    endDate = moment().endOf('day')

                    lineChartOptions.xAxis.min = startDate.valueOf();
                    lineChartOptions.xAxis.max = endDate.valueOf();
                } else {
                    startDate = moment(this.date[0]);
                    endDate = moment(this.date[1]);

                    if(this.date.length === 1 || !this.date[1]) {
                        endDate = moment(this.date[0]).add(7, 'days').endOf('day')    
                    }

                    lineChartOptions.xAxis.min = startDate.valueOf();
                    lineChartOptions.xAxis.max = endDate.valueOf();
                }

                for (const account of this.transactions) {
                    const seriesData = []

                    let currentBalance = account.balance

                    if (account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                        currentBalance = currentBalance * this.currencyRates
                    }

                    // фильтр движений по дате и валюте
                    const filteredMovements = (this.date && this.date.length === 2) ? account.movements.filter(
                        movement => {
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
            },
            // updateBarChart() {
            //     const barChartOptions = {...this.barChartOptions}
            //     let startDateBar, endDateBar
            //     barChartOptions.series = []

            //     if(!this.date || this.date.length !== 2) {
            //         endDateBar = moment().endOf('day')
            //         startDateBar = moment().subtract(7, 'days').startOf('day')
            //     } else {
            //         startDateBar = moment(this.date[0]).startOf('day')
            //         endDateBar = moment(this.date[1]).endOf('day')

            //         if(this.date.length === 1) {
            //             endDateBar = moment(this.date[0]).add(7, 'days').endOf('day')
            //         }
            //     }

            //     // const sortedTransactionsBar = [...this.transactions].sort((a,b) => {
            //     //     const c
            //     // })

            //     const transactionsBar = [...this.transactions]

            //     for(const account of transactionsBar) {
            //         const seriesData = [];
            //         const movements = (this.date && this.date.length === 2) 
            //             ? account.movements.filter(movement => {
            //                 const movementDate = movement(movement.date)
            //                 return movementDate.isBetween(startDateBar, endDateBar, null, '[]')
            //             }) : account.movements

            //         const sortedMovements = movements.sort((a, b) => {
            //             moment(a.date) - moment(b.date)
            //         })

            //         const movementsByDate = {}

            //         for (const movement of sortedMovements) {
            //             const date = moment(movement.date).format('YYYY-MM-DD');

            //             if(!movementsByDate[date]) {
            //                 movementsByDate[date] = []
            //             }

            //             movementsByDate[date].push(movement);

            //             for(const movement of movementsByDate) {
            //                 let income = 0
            //                 let expence = 0


            //             }
  
            //             if(account.idCurrency !== this.selectedCurrency && this.currencyRates) {
            //                 amount = amount * this.currencyRates
            //             }

            //             seriesData.push([
            //                 moment(movement.date).valueOf(),
            //                 amount,
            //                 movement.recipientAccountNumber
            //             ])
            //         }

            //         if(seriesData.length > 0) {
            //             barChartOptions.series.push({
            //                 name: `${account.accountNumber} (${account.currency})`,
            //                 data: seriesData,
            //                 type: 'bar'
            //             })
            //         }

            //         this.barChartOptions = barChartOptions

            //     }
            // }
        }
    }
</script>