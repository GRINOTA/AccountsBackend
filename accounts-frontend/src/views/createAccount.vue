<template>
    <div class="container mt-4 d-flex justify-content-center align-items-start" style="min-height: 100vh">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <legend>Открытие счета</legend>
                </div>
                <div class="card-body">
                    <fieldset>
                        <div class="mb-3">
                            <label for="select" class="form-label">Валюта счета</label>
                            <select id="select" class="form-select" v-model.number="selectedCurrency">
                                <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                                    {{currency.codeCurrency}}
                                </option>
                            </select>
                        </div>
                        <label class="form-label" style="color: red;">{{errorMessage}}</label>
                        <div>
                            <button type="submit" class="btn btn-primary" @click="openAccount">Открыть счет</button>    
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import CurrencyService from '../api/services/currencyService';
    import AccountsService from '../api/services/accountsService';

    export default {
        data() {
            return {
                errorMessage: null,
                currencies: [],
                selectedCurrency: null,
            }
        },
        async mounted() {
            this.currencies = await CurrencyService.getAllCurrency()
        },
        methods: {
            async openAccount() {
                try {
                    await AccountsService.createAccount(this.selectedCurrency)
                    this.$router.replace('/')
                    this.$toast.success('Счет открыт')
                } catch(error) {
                    console.log(error)
                    if(error.response.status === 500) {
                        this.$toast.error(`${error.response.data.message}`)
                    } else {
                        this.$toast.error(`${error.status} ${error.statisText}`)
                    }
                }
            },
        }
    }
</script>