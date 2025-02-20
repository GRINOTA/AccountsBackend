<template>
    <div class="container mt-4">
        <div class="card">
            <div class="card-header">
                <legend>Отчет по движениям средств</legend>
            </div>
            <div class="card-body">
                <div class="mb-3">
                    <label for="selectCurrency" class="form-label">Конвертировать отчет в валюту</label>
                    <select id="selectCurrency" class="form-select" v-model.number="selectedCurrency">
                        <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                            {{currency.codeCurrency}}
                        </option>
                    </select>
                </div>

                <div class="mt-4">  
                    <p class="h5">Фильтрация</p>
                    <div class="mb-3"> 
                        <label for="selectDate" class="form-label">По датам</label>
                        <VueDatePicker id="selectDate" v-model="date" range></VueDatePicker>
                    </div>
                    <div class="mb-3"> 
                        <label for="selectCurrencyFiltr" class="form-label">По валютам</label>
                        <select id="selectCurrencyFiltr" class="form-select" v-model.number="selectedCurrencyFilter">
                            <option :value="0"></option>
                            <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                                {{currency.codeCurrency}}
                            </option>
                        </select>
                    </div> 
                </div>

                <div class="mt-4">
                    <div class="row justify-content-center">
                        <line-chart :options="lineChartOptions" @chart-ready="handleChartReady" ref="lineChart"></line-chart>
                    </div>
                </div>  

            </div>
            
            
                
            
        </div>
    </div>
</template>

<script>
    import LineChart from '../components/LineChart.vue'
    import TransactionService from '../api/services/transactionService';
    import moment from 'moment'
    import CurrencyService from '../api/services/currencyService'
    import CurrencyRatesService from '../api/services/currencyRatesService'
    import VueDatePicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css'

    export default {
        components: { 
            LineChart,
            VueDatePicker
        },
        data() {
            return {
                transactions: [],

                selectedCurrency:  1,
                selectedCurrencyFilter: 0,
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
                }
            }
        },
        watch: {
            selectedCurrency: {
                handler: async function() {
                    await this.initData()
                }
            },
            date: {
                handler: async function() {
                    await this.initData()
                }
            },
            selectedCurrencyFilter: {
                handler: async function() {
                    await this.initData()
                }
            }
        },
        async mounted() {
            await this.initData()
        },

        methods: {
            async initData() {
                await this.getCurrencies()
                await this.getTransaction()
                this.updateLineChart()
            },
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
            },
            handleChartReady() {
                this.updateLineChart()
                console.log("LineChart ref:", this.$refs.lineChart);
                console.log("LineChart chart:", this.$refs.lineChart && this.$refs.lineChart.chart);
            },
            updateLineChart() {
                const lineChartOptions = { ...this.lineChartOptions }

                let startDate, endDate

                const newSeries = []

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
                let filteredTransaction = this.transactions
                if(this.selectedCurrencyFilter !== 0) {
                    filteredTransaction = this.transactions.filter(account => account.idCurrency === this.selectedCurrencyFilter)
                }
                console.log(filteredTransaction)
                
                for (const account of filteredTransaction) {

                    // const isAccountCurrencyMatching = this.selectedCurrencyFilter === 0 || account.idCurrency === this.selectedCurrencyFilter 
                    
                    // фильтрация по валюте
                    // if(!isAccountCurrencyMatching) {
                    //     continue
                    // }

                    const seriesData = []

                    let currentBalance = account.balance
                    
                    if (account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                        currentBalance = currentBalance * this.currencyRates
                    }

                    // фильтр движений по дате
                    const filteredMovements = ((this.date && this.date.length === 2)) ? account.movements.filter(
                        movement => {
                            const movementDate = moment(movement.date)                            
                            return movementDate.isBetween(startDate, endDate, null, '[]');
                    }) : account.movements;

                    const sortedMovements = filteredMovements.sort((a, b) => {
                        const dateA = moment(a.date)
                        const dateB = moment(b.date)
                        return dateB - dateA
                    });
 
                    for (const movement of sortedMovements) {
                        if(movement.idCurrency !== this.selectedCurrencyFilter && this.selectedCurrencyFilter !== 0) {
                            continue
                        }
                        
                        let amount = movement.amount
                        
                        if(account.idCurrency !== this.selectedCurrency && this.currencyRates) {
                            amount = amount * this.currencyRates
                        }

                        currentBalance -= amount

                        seriesData.push([
                            moment(movement.date).valueOf(),
                            currentBalance,
                            movement.recipientAccountNumber,
                            amount
                        ])
                        
                    }
                    console.log("series", seriesData)
                    if(seriesData.length > 0) {
                        newSeries.push({
                            name: `${account.accountNumber} (${account.currency})`,  
                            data: seriesData,
                            type: 'line'  
                        })
                    }
                }
                console.log("newSeries", newSeries)
                lineChartOptions.series = newSeries;

                lineChartOptions.legend.data = lineChartOptions.series.map(series => series.name);
                
                this.lineChartOptions = {...lineChartOptions}

                this.$nextTick(() => {
                    if (this.$refs.lineChart && this.$refs.lineChart.chart) {
                        console.log(lineChartOptions.series);
                        this.$refs.lineChart.chart.setOption(this.lineChartOptions);
                    } else {
                        console.error("График не инициализирован или ref не найден.");
                    }
                });
            }
        }
    }
</script>