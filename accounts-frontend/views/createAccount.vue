<template>
    <div>
        <fieldset>
            <legend>Открытие счета</legend>
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
</template>

<script>
    import CurrencyService from '../services/currencyService';
    import AccountsService from '../services/accountsService';

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
                await AccountsService.createAccount(this.selectedCurrency)
                // if(responce < 400) {
                    // this.$router.replace('/')
                    // this.$toast.success('Счет открыт')
                    // if(responce.status === 500) {
                    //     this.$toast.error(`${responce.data.message}`)
                    // }
                    
                // } else {
                //     this.$toast.error(`${responce}`) 
                //     this.errorMessage = responce
                // }
            },
        }
    }
</script>