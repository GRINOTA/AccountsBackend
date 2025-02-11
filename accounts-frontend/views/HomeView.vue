<template>
    <div>
        <div style="display: flex; margin-bottom: 1%;">
            <legend>Мои счета</legend>
            <button type="button" class="btn btn-dark" @click="logout">Выйти</button>
        </div>
        <div style="display: flex; justify-content: center; align-items: center;">
            <table class="table table-bordered border-primary">
                <thead class="table-light">
                    <tr>
                        <th class="text-center">Номер</th>
                        <th class="text-center">Баланс</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="account in accounts" :key="account.number">
                        <td class="text-center">{{account.number}}</td>
                        <td class="text-center">{{account.balance}} {{account.currency}}</td>
                    </tr>
                </tbody>
            </table> 
        </div>
        
        <div style="display: flex; gap: 2%; justify-content: center;">
            <button type="button" class="btn btn-primary" @click="goCreate">Открыть новый счёт</button>
            <button type="button" class="btn btn-primary" @click="goTransaction">Перевод</button>
            <button type="button" class="btn btn-primary">Отчёт</button>    
        </div>
    </div>
</template>

<script>
    import AccountsService from '../services/accountsService'
    // import AuthService from '../services/authService'
    
    export default {
        data() {
            return {
                accounts: []
            }
        },
        async mounted() {
            this.accounts = await AccountsService.getAccountsList()
        },
        methods: {
            logout() {
                // AuthService.logout()
                this.$router.push('/login')
            },
            goCreate() {
                this.$router.push('/create')
            },
            goTransaction() {
                this.$router.push('/transaction')
            }
        }
    }

</script>
