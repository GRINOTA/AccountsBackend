<template>
    <div class="container mt-4">
        <div class="card">
            <div class="card-header">
                <div class="d-flex justify-content-between">
                    <legend class="mb-0">Мои счета</legend>
                    <button type="button" class="btn btn-primary" @click="logout">Выйти</button>
                </div>
            </div>
            <div class="card-body">
                <div class="d-flex justify-content-center">
                    <table class="table table-bordered border-primary col-md-8">
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
            </div>
        </div>
        
        <div class="d-flex justify-content-center mt-4">
            <div class="btn-group">
                <button type="button" class="btn btn-primary me-2" @click="goCreate">Открыть новый счёт</button>
                <button type="button" class="btn btn-primary me-2" @click="goTransaction">Перевод</button>
                <button type="button" class="btn btn-primary" @click="goReport">Отчёт</button>   
            </div>
        </div>
    </div>
</template>

<script>
    import AccountsService from '../api/services/accountsService'
    import AuthService from '../api/services/authService'
    
    export default {
        data() {
            return {
                accounts: []
            }
        },
        async mounted() {
            try {
                this.accounts = await AccountsService.getAccountsList()
            } catch(error) {
                console.log(error)
                this.$toast.error(`${error.response.status} ${error.response.statusText}`)
            }
        },
        methods: {
            async logout() {
                await AuthService.logout()
                this.$router.replace('/login')
            },
            goCreate() {
                this.$router.push('/create')
            },
            goTransaction() {
                this.$router.push('/transaction')
            },
            goReport() {
                this.$router.push('/report')
            }
        }
    }
</script>
